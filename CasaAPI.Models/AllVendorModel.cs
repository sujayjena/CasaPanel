using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class AllVendorModel
    {
        public class AllVendorSaveParameters
        {
            public int Id { get; set; }
            public int VendorId { get; set; }
            public int CompanyTypeId { get; set; }
            public int SubVendorId { get; set; }
            public int VendorGroupId { get; set; }
            public bool IsActive { get; set; }
        }
        public class AllVendorSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class AllVendorDetailsResponse : LogParameters
        {
            public int Id { get; set; }
            public int VendorId { get; set; }
            public string VendorType { get; set; }
            public int CompanyTypeId { get; set; }
            public string CompanyType { get; set; }
            public int SubVendorId { get; set; }
            public string SubVendor { get; set; }
            public int VendorGroupId { get; set; }
            public string VendorGroup { get; set; }


        }

        public class AllVendorImportSaveParameters
        {
            public string VendorGroup { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
            public string Vendor { get; set; }
            public string CompanyType { get; set; }
            public string SubVendor { get; set; }           
        }
        public class AllVendorFailToImportValidationErrors
        {
            public string IsActive { get; set; }
            public string Vendor { get; set; }
            public string CompanyType { get; set; }
            public string SubVendor { get; set; }
            public string VendorGroup { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
