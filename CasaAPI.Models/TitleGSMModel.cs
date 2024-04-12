using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class TitleGSMModel
    {
        public class TitleGSMSaveParameters
        {
            public int TitleGSMId { get; set; }
            [Required(ErrorMessage = ValidationConstants.TitleGSMNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TitleGSMNameRegExp, ErrorMessage = ValidationConstants.TitleGSMNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TitleGSMName_MaxLength, ErrorMessage = ValidationConstants.TitleGSMName_MaxLength_Msg)]
            public string TitleGSMName { get; set; }
            public bool IsActive { get; set; }
        }
        public class TitleGSMDetailsResponse : LogParameters
        {
            public int TitleGSMId { get; set; }
            public string TitleGSMName { get; set; }

        }
        public class TitleGSMSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }

            [JsonIgnore]
            public bool? IsExport { get; set; }

        }
        public class TitleGSMImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.TitleGSMNameRequied_Msg)]
            [RegularExpression(ValidationConstants.TitleGSMNameRegExp, ErrorMessage = ValidationConstants.TitleGSMNameRegExp_Msg)]
            [MaxLength(ValidationConstants.TitleGSMName_MaxLength, ErrorMessage = ValidationConstants.TitleGSMName_MaxLength_Msg)]
            public string TitleGSMName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class TitleGSMFailToImportValidationErrors
        {
            public string TitleGSMName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
