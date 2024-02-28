using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class VendorGroupModel
    {
        public class VendorGroupSaveParameters
        {
            public int VendorGroupId { get; set; }
            [Required(ErrorMessage = ValidationConstants.VendorGroupRequied_Msg)]
            [RegularExpression(ValidationConstants.VendorGroupRegExp, ErrorMessage = ValidationConstants.VendorGroupRegExp_Msg)]
            [MaxLength(ValidationConstants.VendorGroup_MaxLength, ErrorMessage = ValidationConstants.VendorGroup_MaxLength_Msg)]
            public string VendorGroup { get; set; }
            public bool IsActive { get; set; }
        }
        public class VendorGroupDetailsResponse : LogParameters
        {
            public int VendorGroupId { get; set; }
            public string VendorGroup { get; set; }

        }
        public class VendorGroupSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class VendorGroupImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.VendorGroupRequied_Msg)]
            [RegularExpression(ValidationConstants.VendorGroupRegExp, ErrorMessage = ValidationConstants.VendorGroupRegExp_Msg)]
            [MaxLength(ValidationConstants.VendorGroup_MaxLength, ErrorMessage = ValidationConstants.VendorGroup_MaxLength_Msg)]
            public string VendorGroup { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class VendorGroupFailToImportValidationErrors
        {
            public string VendorGroup { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
