using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class WeekCloseModel
    {
        public class WeekCloseSaveParameters
        {
            public int WeekCloseId { get; set; }
            [Required(ErrorMessage = ValidationConstants.WeekCloseRequied_Msg)]
            [RegularExpression(ValidationConstants.WeekCloseRegExp, ErrorMessage = ValidationConstants.WeekCloseRegExp_Msg)]
            [MaxLength(ValidationConstants.WeekClose_MaxLength, ErrorMessage = ValidationConstants.WeekClose_MaxLength_Msg)]
            public string WeekClose { get; set; }
            public bool IsActive { get; set; }
        }
        public class WeekCloseDetailsResponse : LogParameters
        {
            public int WeekCloseId { get; set; }
            public string WeekClose { get; set; }

        }
        public class WeekCloseSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class WeekCloseImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.WeekCloseRequied_Msg)]
            [RegularExpression(ValidationConstants.WeekCloseRegExp, ErrorMessage = ValidationConstants.WeekCloseRegExp_Msg)]
            [MaxLength(ValidationConstants.WeekClose_MaxLength, ErrorMessage = ValidationConstants.WeekClose_MaxLength_Msg)]
            public string WeekClose { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class WeekCloseFailToImportValidationErrors
        {
            public string WeekClose { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
