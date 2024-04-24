using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class TitleProcessModel
    {
        public class TitleProcessSaveParameters
        {
            public int TitleProcessId { get; set; }
            [Required(ErrorMessage = ValidationConstants.TitleProcessNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TitleProcessNameRegExp, ErrorMessage = ValidationConstants.TitleProcessNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TitleProcessName_MaxLength, ErrorMessage = ValidationConstants.TitleProcessName_MaxLength_Msg)]
            public string TitleProcessName { get; set; }
            public bool IsActive { get; set; }
        }
        public class TitleProcessDetailsResponse : LogParameters
        {
            public int TitleProcessId { get; set; }
            public string TitleProcessName { get; set; }

        }
        public class TitleProcessSearchParameters
        {
            public PaginationParameters pagination { get; set; }

            [DefaultValue("")]
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }

            [JsonIgnore]
            public bool? IsExport { get; set; }

        }
        public class TitleProcessImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.TitleProcessNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TitleProcessNameRegExp, ErrorMessage = ValidationConstants.TitleProcessNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TitleProcessName_MaxLength, ErrorMessage = ValidationConstants.TitleProcessName_MaxLength_Msg)]
            public string TitleProcessName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class TitleProcessFailToImportValidationErrors
        {
            public string TitleProcessName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
