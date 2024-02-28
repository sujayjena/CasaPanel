using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class TileSizeModel
    {
        public class TileSizeSaveParameters
        {
            public int TileSizeId { get; set; }
            [Required(ErrorMessage = ValidationConstants.TileSizeNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TileSizeNameRegExp, ErrorMessage = ValidationConstants.TileSizeNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TileSizeName_MaxLength, ErrorMessage = ValidationConstants.TileSizeName_MaxLength_Msg)]
            public string TileSizeName { get; set; }
            public bool IsActive { get; set; }
        }
        public class TileSizeDetailsResponse : LogParameters
        {
            public int TileSizeId { get; set; }
            public string TileSizeName { get; set; }

        }
        public class TileSizeSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; }
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }
        }
        public class TileSizeImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.TileSizeNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TileSizeNameRegExp, ErrorMessage = ValidationConstants.TileSizeNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TileSizeName_MaxLength, ErrorMessage = ValidationConstants.TileSizeName_MaxLength_Msg)]
            public string TileSizeName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class TileSizeFailToImportValidationErrors
        {
            public string TileSizeName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
