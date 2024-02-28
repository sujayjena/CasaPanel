using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class ThicknessModel
    {
        public class ThicknessSaveParameters
        {
            public int ThicknessId { get; set; }
            [Required(ErrorMessage = ValidationConstants.ThicknessNameRequied_Msg)]
            [RegularExpression(ValidationConstants.ThicknessNameRegExp, ErrorMessage = ValidationConstants.ThicknessNameRegExp_Msg)]
            [MaxLength(ValidationConstants.ThicknessName_MaxLength, ErrorMessage = ValidationConstants.ThicknessName_MaxLength_Msg)]
            public string ThicknessName { get; set; }
            public bool IsActive { get; set; }
        }
        public class ThicknessDetailsResponse : LogParameters
        {
            public int ThicknessId { get; set; }
            public string ThicknessName { get; set; }

        }
        public class ThicknessSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; }
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }
        }
        public class ThicknessImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.ThicknessNameRequied_Msg)]
            [RegularExpression(ValidationConstants.ThicknessNameRegExp, ErrorMessage = ValidationConstants.ThicknessNameRegExp_Msg)]
            [MaxLength(ValidationConstants.ThicknessName_MaxLength, ErrorMessage = ValidationConstants.ThicknessName_MaxLength_Msg)]
            public string ThicknessName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class ThicknessFailToImportValidationErrors
        {
            public string ThicknessName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
