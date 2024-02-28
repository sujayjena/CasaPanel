using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class TDSNatureModel
    {
        public class TDSNatureSaveParameters
        {
            public int TDSNatureId { get; set; }
            [Required(ErrorMessage = ValidationConstants.TDSNaturRegExp_Msg)]
            [RegularExpression(ValidationConstants. TDSNaturRegExp, ErrorMessage = ValidationConstants.TDSNaturRegExp_Msg)]
            [MaxLength(ValidationConstants.TDSNatur_MaxLength, ErrorMessage = ValidationConstants.TDSNatur_MaxLength_Msg)]
            public string TDSNature { get; set; }
            public bool IsActive { get; set; }
        }
        public class TDSNatureDetailsResponse : LogParameters
        {
            public int TDSNatureId { get; set; }
            public string TDSNature { get; set; }

        }
        public class TDSNatureSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class TDSNatureImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.TDSNaturRegExp_Msg)]
            [RegularExpression(ValidationConstants.TDSNaturRegExp, ErrorMessage = ValidationConstants.TDSNaturRegExp_Msg)]
            [MaxLength(ValidationConstants.TDSNatur_MaxLength, ErrorMessage = ValidationConstants.TDSNatur_MaxLength_Msg)]
            public string TDSNature { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class TDSNatureFailToImportValidationErrors
        {
            public string TDSNature { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
