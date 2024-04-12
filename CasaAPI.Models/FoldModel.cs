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
    public class FoldModel
    {
        public class FoldSaveParameters
        {
            public int FoldId { get; set; }
            [Required(ErrorMessage = ValidationConstants.FoldNameRequied_Msg)]
            [RegularExpression(ValidationConstants.FoldNameRegExp, ErrorMessage = ValidationConstants.FoldNameRegExp_Msg)]
            [MaxLength(ValidationConstants.FoldName_MaxLength, ErrorMessage = ValidationConstants.FoldName_MaxLength_Msg)]
            public string FoldName { get; set; }
            public bool IsActive { get; set; }
        }
        public class FoldDetailsResponse : LogParameters
        {
            public int FoldId { get; set; }
            public string FoldName { get; set; }

        }
        public class FoldSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }

            [JsonIgnore]
            public bool? IsExport { get; set; }

        }
        public class FoldImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.FoldNameRequied_Msg)]
            [RegularExpression(ValidationConstants.FoldNameRegExp, ErrorMessage = ValidationConstants.FoldNameRegExp_Msg)]
            [MaxLength(ValidationConstants.FoldName_MaxLength, ErrorMessage = ValidationConstants.FoldName_MaxLength_Msg)]
            public string FoldName { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class FoldFailToImportValidationErrors
        {
            public string FoldName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
