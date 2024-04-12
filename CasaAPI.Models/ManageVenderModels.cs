using CasaAPI.Models.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class VenderSaveParameters
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = ValidationConstants.RoleNameRequied_Msg)]
        //[RegularExpression(ValidationConstants.RoleNameRegExp, ErrorMessage = ValidationConstants.RoleNameRegExp)]
        //[MaxLength(ValidationConstants.RoleName_MaxLength, ErrorMessage = ValidationConstants.RoleName_MaxLength_Msg)]
       
        public DateTime? Date { get; set; }
        public int? VendorType { get; set; }
        public int? SubVendorType { get; set; }
        public string VendorName { get; set; }
        public decimal? TelephpneNo { get; set; }
        public string FaxNo { get; set; }
        public decimal? MobileNumber { get; set; }
        public string Email { get; set; }
        public int? CompanyType { get; set; }
        public string Website { get; set; }
        public int? ReferenceBy { get; set; }
        public string PanNumber { get; set; }
        public string UploadPanCard { get; set; }
        public IFormFile? UploadPanCardFile { get; set; }
        public string GSTNumber { get; set; }
        public string UploadGSTNumber { get; set; }
        public IFormFile? UploadGSTNumberFile { get; set; }
        public bool? ImportExportCertificate { get; set; }
        public string IECCode { get; set; }
        public string UploadICECertificate { get; set; }
        public IFormFile? UploadICECertificateFile { get; set; }
        public string UploadNameChangeCertificate { get; set; }
        public IFormFile? UploadNameChangeCertificateFile { get; set; }
        public bool? IsTheNameChangeDone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ContactType { get; set; }
        public decimal? AlternativeNumber { get; set; }
        public decimal? ContactTelephoneNo { get; set; }
        public string ContactFaxNo { get; set; }
        public decimal? ContactMobileNumber { get; set; }
        public string ContactEmailId { get; set; }
        public decimal? BankAcNo { get; set; }
        public string NameOfBank { get; set; }
        public string BankAddress { get; set; }
        public string BankBranchName { get; set; }
        public string IFSCCode { get; set; }
        public string MICRCode { get; set; }
        public string CancelledChequeNumber { get; set; }
        public string UploadCanceledcheque { get; set; }
        public IFormFile? UploadCanceledchequeFile { get; set; }
        public int? vendorPostingGroup { get; set; }
        public string WorkingHours { get; set; }
        public string PaymentTerme { get; set; }
        public string ModeOfTransportation { get; set; }
        public string DistancefromOurFactoryKN { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
    }

    public class VenderDetailsResponse : LogParameters
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public int VendorType { get; set; }
        public int SubVendorType { get; set; }
        public string VendorTypeName { get; set; }
        public string SubVendorTypeName { get; set; }
        public string VendorName { get; set; }
        public decimal TelephpneNo { get; set; }
        public string FaxNo { get; set; }
        public decimal MobileNumber { get; set; }
        public string Email { get; set; }
        public int CompanyType { get; set; }
        public string CompanyTypeName { get; set; }
        public string Website { get; set; }
        public int ReferenceBy { get; set; }
        public string ReferenceByName { get; set; }
        public string PanNumber { get; set; }
        public string UploadPanCard { get; set; }
        public string GSTNumber { get; set; }
        public string UploadGSTNumber { get; set; }
        public bool ImportExportCertificate { get; set; }
        public string IECCode { get; set; }
        public string UploadICECertificate { get; set; }
        public string UploadNameChangeCertificate { get; set; }
        public bool IsTheNameChangeDone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ContactType { get; set; }
        public string ContactTypeName { get; set; }
        public decimal AlternativeNumber { get; set; }
        public decimal ContactTelephoneNo { get; set; }
        public string ContactFaxNo { get; set; }
        public decimal ContactMobileNumber { get; set; }
        public string ContactEmailId { get; set; }
        public decimal BankAcNo { get; set; }
        public string NameOfBank { get; set; }
        public string BankAddress { get; set; }
        public string BankBranchName { get; set; }
        public string IFSCCode { get; set; }
        public string MICRCode { get; set; }
        public string CancelledChequeNumber { get; set; }
        public string UploadCanceledcheque { get; set; }
        public int vendorPostingGroup { get; set; }
        public string vendorPostingGroupName { get; set; }
        public string WorkingHours { get; set; }
        public string PaymentTerme { get; set; }
        public string ModeOfTransportation { get; set; }
        public string DistancefromOurFactoryKN { get; set; }
        public string StatusName { get; set; }
        public int Status { get; set; }

    }
    public class VenderSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;

        public bool? IsActive { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public bool? IsExport { get; set; }
    }

    public class VenderUpdateStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }

    }
}

