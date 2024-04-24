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
    public class InnerGSMModel
    {
        public class InnerGSMSaveParameters
        {
            public int InnerGSMId { get; set; }
            [Required(ErrorMessage = ValidationConstants.InnerGSMNameRequied_Msg)]
            [RegularExpression(ValidationConstants.InnerGSMNameRegExp, ErrorMessage = ValidationConstants.InnerGSMNameRegExp_Msg)]
            [MaxLength(ValidationConstants.InnerGSMName_MaxLength, ErrorMessage = ValidationConstants.InnerGSMName_MaxLength_Msg)]
            public string InnerGSMName { get; set; }
            public bool IsActive { get; set; }
        }
        public class InnerGSMDetailsResponse : LogParameters
        {
            public int InnerGSMId { get; set; }
            public string InnerGSMName { get; set; }

        }
        public class InnerGSMSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }

            [JsonIgnore]
            public bool? IsExport { get; set; }

        }
        public class InnerGSMImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.InnerGSMNameRequied_Msg)]
            [RegularExpression(ValidationConstants.InnerGSMNameRegExp, ErrorMessage = ValidationConstants.InnerGSMNameRegExp_Msg)]
            [MaxLength(ValidationConstants.InnerGSMName_MaxLength, ErrorMessage = ValidationConstants.InnerGSMName_MaxLength_Msg)]
            public string InnerGSMName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class InnerGSMFailToImportValidationErrors
        {
            public string InnerGSMName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
