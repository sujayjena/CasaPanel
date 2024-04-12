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
    public class FlapModel
    {
        public class FlapSaveParameters
        {
            public int FlapId { get; set; }
            [Required(ErrorMessage = ValidationConstants.FlapNameRequied_Msg)]
            [RegularExpression(ValidationConstants.FlapNameRegExp, ErrorMessage = ValidationConstants.FlapNameRegExp_Msg)]
            [MaxLength(ValidationConstants.FlapName_MaxLength, ErrorMessage = ValidationConstants.FlapName_MaxLength_Msg)]
            public string FlapName { get; set; }
            public bool IsActive { get; set; }
        }
        public class FlapDetailsResponse : LogParameters
        {
            public int FlapId { get; set; }
            public string FlapName { get; set; }

        }
        public class FlapSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }

            [JsonIgnore]
            public bool? IsExport { get; set; }

        }
        public class FlapImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.FlapNameRequied_Msg)]
            [RegularExpression(ValidationConstants.FlapNameRegExp, ErrorMessage = ValidationConstants.FlapNameRegExp_Msg)]
            [MaxLength(ValidationConstants.FlapName_MaxLength, ErrorMessage = ValidationConstants.FlapName_MaxLength_Msg)]
            public string FlapName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class FlapFailToImportValidationErrors
        {
            public string FlapName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
