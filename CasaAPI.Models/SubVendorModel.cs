using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class SubVendorModel
    {
        public class SubVendorSaveParameters
        {
            public int SubVendorId { get; set; }
            [Required(ErrorMessage = ValidationConstants.SubVendorTypeRequied_Msg)]
            [RegularExpression(ValidationConstants.SubVendorTypeRegExp, ErrorMessage = ValidationConstants.SubVendorTypeRegExp_Msg)]
            [MaxLength(ValidationConstants.SubVendorType_MaxLength, ErrorMessage = ValidationConstants.SubVendorType_MaxLength_Msg)]
            public string SubVendorType { get; set; }
            public bool IsActive { get; set; }
        }
        public class SubVendorDetailsResponse : LogParameters
        {
            public int SubVendorId { get; set; }
            public string SubVendorType { get; set; }

        }
        public class SubVendorSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class SubVendorImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.SubVendorTypeRequied_Msg)]
            [RegularExpression(ValidationConstants.SubVendorTypeRegExp, ErrorMessage = ValidationConstants.SubVendorTypeRegExp_Msg)]
            [MaxLength(ValidationConstants.SubVendorType_MaxLength, ErrorMessage = ValidationConstants.SubVendorType_MaxLength_Msg)]
            public string SubVendorType { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class SubVendorFailToImportValidationErrors
        {
            public string SubVendorType { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
