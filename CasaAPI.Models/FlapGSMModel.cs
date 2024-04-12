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
    public class FlapGSMModel
    {
        public class FlapGSMSaveParameters
        {
            public int FlapGSMId { get; set; }
            [Required(ErrorMessage = ValidationConstants.FlapGSMNameRequied_Msg)]
            [RegularExpression(ValidationConstants.FlapGSMNameRegExp, ErrorMessage = ValidationConstants.FlapGSMNameRegExp_Msg)]
            [MaxLength(ValidationConstants.FlapGSMName_MaxLength, ErrorMessage = ValidationConstants.FlapGSMName_MaxLength_Msg)]
            public string FlapGSMName { get; set; }
            public bool IsActive { get; set; }
        }
        public class FlapGSMDetailsResponse : LogParameters
        {
            public int FlapGSMId { get; set; }
            public string FlapGSMName { get; set; }

        }
        public class FlapGSMSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }

            [JsonIgnore]
            public bool? IsExport { get; set; }

        }
        public class FlapGSMImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.FlapGSMNameRequied_Msg)]
            [RegularExpression(ValidationConstants.FlapGSMNameRegExp, ErrorMessage = ValidationConstants.FlapGSMNameRegExp_Msg)]
            [MaxLength(ValidationConstants.FlapGSMName_MaxLength, ErrorMessage = ValidationConstants.FlapGSMName_MaxLength_Msg)]
            public string FlapGSMName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class FlapGSMFailToImportValidationErrors
        {
            public string FlapGSMName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
