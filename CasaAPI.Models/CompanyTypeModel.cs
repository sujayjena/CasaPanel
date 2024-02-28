using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class CompanyTypeModel
    {
        public class CompanyTypeSaveParameters
        {
            public int CompanyTypeId { get; set; }
            [Required(ErrorMessage = ValidationConstants.CompanyTypeRequied_Msg)]
            [RegularExpression(ValidationConstants.CompanyTypeRegExp, ErrorMessage = ValidationConstants.CompanyTypeRegExp_Msg)]
            [MaxLength(ValidationConstants.CompanyType_MaxLength, ErrorMessage = ValidationConstants.CompanyType_MaxLength_Msg)]
            public string CompanyType { get; set; }
            public bool IsActive { get; set; }
        }
        public class CompanyTypeDetailsResponse : LogParameters
        {
            public int CompanyTypeId { get; set; }
            public string CompanyType { get; set; }

        }
        public class CompanyTypeSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class CompanyTypeImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.CompanyTypeRequied_Msg)]
            [RegularExpression(ValidationConstants.CompanyTypeRegExp, ErrorMessage = ValidationConstants.CompanyTypeRegExp_Msg)]
            [MaxLength(ValidationConstants.CompanyType_MaxLength, ErrorMessage = ValidationConstants.CompanyType_MaxLength_Msg)]
            public string CompanyType { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class CompanyTypeFailToImportValidationErrors
        {
            public string CompanyType { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
