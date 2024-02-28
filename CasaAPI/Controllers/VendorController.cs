using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Packaging.Ionic.Zlib;
using System.Reflection;

namespace CasaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : CustomBaseController
    {
        private ResponseModel _response;
        private IVendorsService _vendorsService;
        private IFileManager _fileManager;
        public VendorController(IVendorsService vendorsService, IFileManager fileManager)
        {
            _vendorsService = vendorsService;
            _fileManager = fileManager;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region ManageVender
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveManageVender([FromForm] VenderSaveParameters Request)
        {
            if (Request.UploadPanCardFile?.Length > 0)
            {
                Request.UploadPanCard = _fileManager.UploadProfilePicture(Request.UploadPanCardFile);
            }
            if (Request.UploadGSTNumberFile?.Length > 0)
            {
                Request.UploadGSTNumber = _fileManager.UploadProfilePicture(Request.UploadGSTNumberFile);
            }
            if (Request.UploadICECertificateFile?.Length > 0)
            {
                Request.UploadICECertificate = _fileManager.UploadProfilePicture(Request.UploadICECertificateFile);
            }
            if (Request.UploadNameChangeCertificateFile?.Length > 0)
            {
                Request.UploadNameChangeCertificate = _fileManager.UploadProfilePicture(Request.UploadNameChangeCertificateFile);
            }
            if (Request.UploadCanceledchequeFile?.Length > 0)
            {
                Request.UploadCanceledcheque = _fileManager.UploadProfilePicture(Request.UploadCanceledchequeFile);
            }
            int result = await _vendorsService.SaveVender(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Vender Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Vsender details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetManageVenderList(VenderSearchParameters request)
        {
            IEnumerable<VenderDetailsResponse> lstDealer = await _vendorsService.GetVenderList(request);
            List<VenderDetailsResponse> datalist = new List<VenderDetailsResponse>();
            if (lstDealer != null && lstDealer.ToList().Count > 0)
            {
                foreach (VenderDetailsResponse record in lstDealer)
                {
                    VenderDetailsResponse data = new VenderDetailsResponse();
                    data.Id = record.Id;                   
                    data.LastName = record.LastName;
                    data.FirstName = record.FirstName;
                    data.GSTNumber = record.GSTNumber;
                    data.TelephpneNo = record.TelephpneNo;
                    data.NameOfBank = record.NameOfBank;
                    data.ModeOfTransportation = record.ModeOfTransportation;
                    data.BankBranchName = record.BankBranchName;
                    data.UploadGSTNumber=record.UploadGSTNumber;
                    data.ContactType = record.ContactType;
                    data.CreatorName = record.CreatorName;
                    data.CreatedOn = record.CreatedOn;
                    data.IsActive = record.IsActive;
                    data.IECCode= record.IECCode;
                    data.IFSCCode= record.IFSCCode;
                    data.CreatedBy= record.CreatedBy;
                    data.MICRCode= record.MICRCode;
                    data.Status=record.Status;
                    data.StatusName = ((VenderStatusMaster)record.Status).ToString();
                    data.PanNumber= record.PanNumber;
                    data.MobileNumber= record.MobileNumber;
                    data.IsTheNameChangeDone= record.IsTheNameChangeDone;
                    data.AlternativeNumber= record.AlternativeNumber;
                    data.BankAcNo= record.BankAcNo;
                    data.WorkingHours= record.WorkingHours;
                    data.SubVendorType= record.SubVendorType;
                    data.DistancefromOurFactoryKN= record.DistancefromOurFactoryKN;
                    data.BankBranchName= record.BankBranchName;
                    data.CancelledChequeNumber= record.CancelledChequeNumber;
                    data.CompanyType= record.CompanyType;
                    data.VendorType= record.VendorType;
                    data.VendorName= record.VendorName;
                    data.vendorPostingGroup= record.vendorPostingGroup;
                    data.ContactEmailId= record.ContactEmailId;
                    data.ContactFaxNo= record.ContactFaxNo;
                    data.ContactMobileNumber= record.ContactMobileNumber;
                    data.ContactTelephoneNo= record.ContactTelephoneNo;                   
                    data.Date= record.Date;
                    data.Email= record.Email;
                    data.FaxNo= record.FaxNo;
                    data.ImportExportCertificate= record.ImportExportCertificate;
                    data.PaymentTerme= record.PaymentTerme;
                    data.ReferenceBy= record.ReferenceBy;
                    data.Website= record.Website;
                    data.ReferenceByName= record.ReferenceByName;
                    data.SubVendorTypeName= record.SubVendorTypeName;
                    data.CompanyTypeName= record.CompanyTypeName;
                    data.vendorPostingGroupName= record.vendorPostingGroupName;
                    data.ContactTypeName = record.ContactTypeName;
                    data.VendorTypeName = record.VendorTypeName;
                    data.TotalRecords = record.TotalRecords;
                    if (record?.UploadPanCard?.Length > 0)
                    {
                        data.UploadPanCard = Convert.ToBase64String(_fileManager.GetProfilePicture(record.UploadPanCard));
                    }
                    if (record?.UploadGSTNumber?.Length > 0)
                    {
                        data.UploadGSTNumber = Convert.ToBase64String(_fileManager.GetProfilePicture(record.UploadGSTNumber));
                    }
                    if (record?.UploadICECertificate?.Length > 0)
                    {
                        data.UploadICECertificate = Convert.ToBase64String(_fileManager.GetProfilePicture(record.UploadICECertificate));
                    }
                    if (record?.UploadNameChangeCertificate?.Length > 0)
                    {
                        data.UploadNameChangeCertificate = Convert.ToBase64String(_fileManager.GetProfilePicture(record.UploadNameChangeCertificate));
                    }
                    if (record?.UploadCanceledcheque?.Length > 0)
                    {
                        data.UploadCanceledcheque = Convert.ToBase64String(_fileManager.GetProfilePicture(record.UploadCanceledcheque));
                    }

                    datalist.Add(data);
                }
            }
            _response.Data = datalist.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetManageVenderDetails(long id)
        {
            VenderDetailsResponse? vender;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                vender = await _vendorsService.GetVenderDetailsById(id);
                VenderDetailsResponse data = new VenderDetailsResponse();
                if (vender != null)
                {
                    data.Id = vender.Id;
                    data.LastName = vender.LastName;
                    data.FirstName = vender.FirstName;
                    data.GSTNumber = vender.GSTNumber;                    
                    data.ContactType = vender.ContactType;
                    data.CreatorName = vender.CreatorName;
                    data.TelephpneNo = vender.TelephpneNo;
                    data.CreatedOn = vender.CreatedOn;
                    data.IsActive = vender.IsActive;
                    data.IECCode = vender.IECCode;
                    data.IFSCCode = vender.IFSCCode;
                    data.NameOfBank= vender.NameOfBank;
                    data.ModeOfTransportation= vender.ModeOfTransportation;
                    data.BankBranchName= vender.BankBranchName;
                    data.CreatedBy = vender.CreatedBy;
                    data.MICRCode = vender.MICRCode;
                    data.Status = vender.Status;
                    data.StatusName = ((VenderStatusMaster)vender.Status).ToString();
                    data.PanNumber = vender.PanNumber;
                    data.MobileNumber = vender.MobileNumber;
                    data.IsTheNameChangeDone = vender.IsTheNameChangeDone;
                    data.AlternativeNumber = vender.AlternativeNumber;
                    data.BankAcNo = vender.BankAcNo;
                    data.WorkingHours = vender.WorkingHours;
                    data.SubVendorType = vender.SubVendorType;
                    data.DistancefromOurFactoryKN = vender.DistancefromOurFactoryKN;
                    data.BankBranchName = vender.BankBranchName;
                    data.CancelledChequeNumber = vender.CancelledChequeNumber;
                    data.CompanyType = vender.CompanyType;
                    data.VendorType = vender.VendorType;
                    data.VendorName = vender.VendorName;
                    data.vendorPostingGroup = vender.vendorPostingGroup;
                    data.ContactEmailId = vender.ContactEmailId;
                    data.ContactFaxNo = vender.ContactFaxNo;
                    data.ContactMobileNumber = vender.ContactMobileNumber;
                    data.ContactTelephoneNo = vender.ContactTelephoneNo;
                    data.Date = vender.Date;
                    data.Email = vender.Email;
                    data.FaxNo = vender.FaxNo;
                    data.ImportExportCertificate = vender.ImportExportCertificate;
                    data.PaymentTerme = vender.PaymentTerme;
                    data.ReferenceBy = vender.ReferenceBy;
                    data.Website = vender.Website;
                    data.ReferenceByName = vender.ReferenceByName;
                    data.SubVendorTypeName = vender.SubVendorTypeName;
                    data.CompanyTypeName = vender.CompanyTypeName;
                    data.vendorPostingGroupName = vender.vendorPostingGroupName;
                    data.CompanyTypeName = vender.CompanyTypeName;
                    data.VendorTypeName = vender.VendorTypeName;
                    data.TotalRecords = vender.TotalRecords;
                    if (vender?.UploadPanCard?.Length > 0)
                    {
                        data.UploadPanCard = Convert.ToBase64String(_fileManager.GetProfilePicture(vender.UploadPanCard));
                    }
                    if (vender?.UploadGSTNumber?.Length > 0)
                    {
                        data.UploadGSTNumber = Convert.ToBase64String(_fileManager.GetProfilePicture(vender.UploadGSTNumber));
                    }
                    if (vender?.UploadICECertificate?.Length > 0)
                    {
                        data.UploadICECertificate = Convert.ToBase64String(_fileManager.GetProfilePicture(vender.UploadICECertificate));
                    }
                    if (vender?.UploadNameChangeCertificate?.Length > 0)
                    {
                        data.UploadNameChangeCertificate = Convert.ToBase64String(_fileManager.GetProfilePicture(vender.UploadNameChangeCertificate));
                    }
                    if (vender?.UploadCanceledcheque?.Length > 0)
                    {
                        data.UploadCanceledcheque = Convert.ToBase64String(_fileManager.GetProfilePicture(vender.UploadCanceledcheque));
                    }
                    _response.Data = data;
                }
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> UpdateManageVenderStatus(VenderUpdateStatus Request)
        {            
            int result = await _vendorsService.UpdateVenderStatus(Request);
            _response.IsSuccess = false;

            if (result >0)
            {
                _response.IsSuccess = true;
                _response.Message = "Vsender Status Update Sucessfully";
            }
            return _response;
        }
        #endregion
    }
}
