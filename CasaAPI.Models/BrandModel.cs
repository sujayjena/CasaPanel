using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class BrandModel
    {
        public class BrandSaveParameters
        {
            public int BrandId { get; set; }
            [Required(ErrorMessage = ValidationConstants.BrandNameRequied_Msg)]
            [RegularExpression(ValidationConstants.BrandNameRegExp, ErrorMessage = ValidationConstants.BrandNameRegExp_Msg)]
            [MaxLength(ValidationConstants.BrandName_MaxLength, ErrorMessage = ValidationConstants.BrandName_MaxLength_Msg)]
            public string BrandName { get; set; }
            public bool IsActive { get; set; }
        }
        public class BrandDetailsResponse : LogParameters
        {
            public int BrandId { get; set; }
            public string BrandName { get; set; }

        }
        public class BrandSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class BrandImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.BrandNameRequied_Msg)]
            [RegularExpression(ValidationConstants.BrandNameRegExp, ErrorMessage = ValidationConstants.BrandNameRegExp_Msg)]
            [MaxLength(ValidationConstants.BrandName_MaxLength, ErrorMessage = ValidationConstants.BrandName_MaxLength_Msg)]
            public string BrandName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class BrandFailToImportValidationErrors
        {
            public string BrandName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
