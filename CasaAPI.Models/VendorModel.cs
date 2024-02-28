using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class VendorModel
    {
        public class VendorSaveParameters
        {
            public int VendorId { get; set; }
            [Required(ErrorMessage = ValidationConstants.VendorTypeRequied_Msg)]
            [RegularExpression(ValidationConstants.VendorTypeRegExp, ErrorMessage = ValidationConstants.VendorTypeRegExp_Msg)]
            [MaxLength(ValidationConstants.VendorType_MaxLength, ErrorMessage = ValidationConstants.VendorType_MaxLength_Msg)]
            public string VendorType { get; set; }
            public bool IsActive { get; set; }
        }
        public class VendorDetailsResponse : LogParameters
        {
            public int VendorId { get; set; }
            public string VendorType { get; set; }

        }
        public class VendorSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class VendorImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.VendorTypeRequied_Msg)]
            [RegularExpression(ValidationConstants.VendorTypeRegExp, ErrorMessage = ValidationConstants.VendorTypeRegExp_Msg)]
            [MaxLength(ValidationConstants.VendorType_MaxLength, ErrorMessage = ValidationConstants.VendorType_MaxLength_Msg)]
            public string VendorType { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class VendorFailToImportValidationErrors
        {
            public string VendorType { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }

    }
}
