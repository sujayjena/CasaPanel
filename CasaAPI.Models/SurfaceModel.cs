using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class SurfaceModel
    {
        public class SurfaceSaveParameters
        {
            public int SurfaceId { get; set; }
            [Required(ErrorMessage = ValidationConstants.SurfaceNameRequied_Msg)]
            [RegularExpression(ValidationConstants.SurfaceNameRegExp, ErrorMessage = ValidationConstants.SurfaceNameRegExp_Msg)]
            [MaxLength(ValidationConstants.SurfaceName_MaxLength, ErrorMessage = ValidationConstants.SurfaceName_MaxLength_Msg)]
            public string SurfaceName { get; set; }
            public bool IsActive { get; set; }
        }
        public class SurfaceDetailsResponse : LogParameters
        {
            public int SurfaceId { get; set; }
            public string SurfaceName { get; set; }

        }
        public class SurfaceSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; }
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class SurfaceImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.SurfaceNameRequied_Msg)]
            [RegularExpression(ValidationConstants.SurfaceNameRegExp, ErrorMessage = ValidationConstants.SurfaceNameRegExp_Msg)]
            [MaxLength(ValidationConstants.SurfaceName_MaxLength, ErrorMessage = ValidationConstants.SurfaceName_MaxLength_Msg)]
            public string SurfaceName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class SurfaceFailToImportValidationErrors
        {
            public string SurfaceName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
