using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class PunchModel
    {
        public class PunchSaveParameters
        {
            public int PunchId { get; set; }
            [Required(ErrorMessage = ValidationConstants.PunchNameRequied_Msg)]
            [RegularExpression(ValidationConstants.PunchNameRegExp, ErrorMessage = ValidationConstants.PunchNameRegExp_Msg)]
            [MaxLength(ValidationConstants.PunchName_MaxLength, ErrorMessage = ValidationConstants.PunchName_MaxLength_Msg)]
            public string PunchName { get; set; }
            public bool IsActive { get; set; }
        }
        public class PunchDetailsResponse : LogParameters
        {
            public int PunchId { get; set; }
            public string PunchName { get; set; }

        }
        public class PunchSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; }
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class PunchImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.PunchNameRequied_Msg)]
            [RegularExpression(ValidationConstants.PunchNameRegExp, ErrorMessage = ValidationConstants.PunchNameRegExp_Msg)]
            [MaxLength(ValidationConstants.PunchName_MaxLength, ErrorMessage = ValidationConstants.PunchName_MaxLength_Msg)]
            public string PunchName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class PunchFailToImportValidationErrors
        {
            public string PunchName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
