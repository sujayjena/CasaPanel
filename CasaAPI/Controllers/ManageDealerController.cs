using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using CasaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Packaging.Ionic.Zlib;

namespace CasaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageDealerController : CustomBaseController
    {
        private ResponseModel _response;
        private IDealerService _dealerService;
        private IFileManager _fileManager;
        public ManageDealerController(IDealerService dealerService, IFileManager fileManager)
        {
            _dealerService = dealerService;
            _fileManager = fileManager;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
        #region Dealer
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDealer([FromForm] DealerSaveParameters Request)
        {

            if (Request.BusinessCardUploadfiles?.Length > 0)
            {
                Request.BusinessCardUpload = _fileManager.UploadProfilePicture(Request.BusinessCardUploadfiles);
            }
            if (Request.ImageUploadfiles?.Length > 0)
            {
                Request.ImageUpload = _fileManager.UploadProfilePicture(Request.ImageUploadfiles);
            }
            if (Request.UploadDealershowroomfiles?.Length > 0)
            {
                Request.Dealershowroom = _fileManager.UploadProfilePicture(Request.UploadDealershowroomfiles);
            }
            int result = await _dealerService.SaveDealer(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Dealer Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Dealer details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDealerList(DealerSearchParameters request)
        {
            IEnumerable<DealerDetailsResponse> lstDealer = await _dealerService.GetDealerList(request);
            List<DealerDetailsResponse> datalist = new List<DealerDetailsResponse>();
            if (lstDealer != null && lstDealer.ToList().Count > 0)
            {
                foreach (DealerDetailsResponse record in lstDealer)
                {
                    DealerDetailsResponse data = new DealerDetailsResponse();
                    data.Id = record.Id;
                    data.Area = record.Area;
                    data.AadhaarNumber = record.AadhaarNumber;
                    data.EmailId = record.EmailId;
                    data.DistanceFromShowroomToGodown= record.DistanceFromShowroomToGodown;
                    data.MobileNumber = record.MobileNumber;
                    data.Anniversarydate = record.Anniversarydate;
                    data.AnyOtherBranchDealing = record.AnyOtherBranchDealing;
                    data.AreaName = record.AreaName;
                    data.BloodGroup = record.BloodGroup;
                    data.BloodGroupName = record.BloodGroupName;
                    data.City = record.City;
                    data.Rating = record.Rating;
                    data.Pincode = record.Pincode;
                    data.PresentShowroomSqFt = record.PresentShowroomSqFt;
                    data.PresentGodownSqFt = record.PresentGodownSqFt;
                    data.Gender = record.Gender;
                    data.GenderName = record.GenderName;
                    data.PANNumber = record.PANNumber;
                    data.LastName = record.LastName;
                    data.FirstName = record.FirstName;
                    data.GSTNumber = record.GSTNumber;
                    data.CityName = record.CityName;
                    data.CompanyDealingaddress = record.CompanyDealingaddress;
                    data.CompanyName = record.CompanyName;
                    data.ContactType = record.ContactType;
                    data.ContactTypeName = record.ContactTypeName;
                    data.CreatorName = record.CreatorName;
                    data.CreatedOn = record.CreatedOn;
                    data.IsActive = record.IsActive;
                    data.District = record.District;
                    data.DistrictName = record.DistrictName;
                    data.State = record.State;
                    data.StateName = record.StateName;
                    data.TileType = record.TileType;
                    data.TileTypeName = record.TileTypeName;
                    data.WeekCloseOrOffDayInMarket = record.WeekCloseOrOffDayInMarket;
                    data.WeekCloseOrOffDayInMarketName = record.WeekCloseOrOffDayInMarketName;
                    data.RegionName = record.RegionName;
                    data.Region = record.Region;
                    data.DateofBirth = record.DateofBirth;
                    data.Dealershowroom = record.Dealershowroom;
                    data.EmergencyContactNumber = record.EmergencyContactNumber;
                    data.SegmentType = record.SegmentType;
                    data.SegmentTypeName = record.SegmentTypeName;
                    data.SpaceProvidedToCase = record.SpaceProvidedToCase;
                    data.Showroom = record.Showroom;
                    data.PresentShowroomSqFt = record.PresentShowroomSqFt;
                    data.CurrentHandlingBrandOrCompany = record.CurrentHandlingBrandOrCompany;
                    data.StatusId = record.StatusId;
                    data.StatusName = record.StatusName;
                    data.TotalRecords = record.TotalRecords;

                    if (record?.BusinessCardUpload?.Length > 0)
                    {
                        data.BusinessCardUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(record.BusinessCardUpload));
                    }
                    if (record?.Dealershowroom?.Length > 0)
                    {
                        data.Dealershowroom = Convert.ToBase64String(_fileManager.GetProfilePicture(record.Dealershowroom));
                    }
                    if (record?.ImageUpload?.Length > 0)
                    {
                        data.ImageUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(record.ImageUpload));
                    }

                    datalist.Add(data);
                }
            }
            _response.Data = datalist.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDealerDetails(long id)
        {
            DealerDetailsResponse? dealer;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                dealer = await _dealerService.GetDealerDetailsById(id);
                DealerDetailsResponse data = new DealerDetailsResponse();
                if (dealer != null)
                {
                    data.Id = dealer.Id;
                    data.Area = dealer.Area;
                    data.AadhaarNumber = dealer.AadhaarNumber;
                    data.MobileNumber = dealer.MobileNumber;
                    data.Anniversarydate = dealer.Anniversarydate;
                    data.AnyOtherBranchDealing = dealer.AnyOtherBranchDealing;
                    data.AreaName = dealer.AreaName;
                    data.BloodGroup = dealer.BloodGroup;
                    data.BloodGroupName = dealer.BloodGroupName;
                    data.City = dealer.City;
                    data.Rating = dealer.Rating;
                    data.Pincode = dealer.Pincode;
                    data.PresentShowroomSqFt = dealer.PresentShowroomSqFt;
                    data.PresentGodownSqFt = dealer.PresentGodownSqFt;
                    data.Gender = dealer.Gender;
                    data.GenderName = dealer.GenderName;
                    data.PANNumber = dealer.PANNumber;
                    data.LastName = dealer.LastName;
                    data.FirstName = dealer.FirstName;
                    data.GSTNumber = dealer.GSTNumber;
                    data.CityName = dealer.CityName;
                    data.CompanyDealingaddress = dealer.CompanyDealingaddress;
                    data.CompanyName = dealer.CompanyName;
                    data.ContactType = dealer.ContactType;
                    data.ContactTypeName = dealer.ContactTypeName;
                    data.CreatorName = dealer.CreatorName;
                    data.CreatedOn = dealer.CreatedOn;
                    data.IsActive = dealer.IsActive;
                    data.District = dealer.District;
                    data.DistrictName = dealer.DistrictName;
                    data.State = dealer.State;
                    data.StateName = dealer.StateName;
                    data.TileType = dealer.TileType;
                    data.TileTypeName = dealer.TileTypeName;
                    data.WeekCloseOrOffDayInMarket = dealer.WeekCloseOrOffDayInMarket;
                    data.WeekCloseOrOffDayInMarketName = dealer.WeekCloseOrOffDayInMarketName;
                    data.RegionName = dealer.RegionName;
                    data.Region = dealer.Region;
                    data.DateofBirth = dealer.DateofBirth;
                    //data.Dealershowroom = dealer.Dealershowroom;
                    data.EmergencyContactNumber = dealer.EmergencyContactNumber;
                    data.SegmentType = dealer.SegmentType;
                    data.SegmentTypeName = dealer.SegmentTypeName;
                    data.SpaceProvidedToCase = dealer.SpaceProvidedToCase;
                    data.Showroom = dealer.Showroom;
                    data.PresentShowroomSqFt = dealer.PresentShowroomSqFt;
                    data.EmailId = dealer.EmailId;
                    data.DistanceFromShowroomToGodown = dealer.DistanceFromShowroomToGodown;
                    data.CurrentHandlingBrandOrCompany = dealer.CurrentHandlingBrandOrCompany;
                    data.StatusId = dealer.StatusId;
                    data.StatusName = dealer.StatusName;

                    if (dealer?.BusinessCardUpload?.Length > 0)
                    {
                        data.BusinessCardUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(dealer.BusinessCardUpload));
                    }
                    if (dealer?.Dealershowroom?.Length > 0)
                    {
                        data.Dealershowroom = Convert.ToBase64String(_fileManager.GetProfilePicture(dealer.Dealershowroom));
                    }
                    if (dealer?.ImageUpload?.Length > 0)
                    {
                        data.ImageUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(dealer.ImageUpload));
                    }
                    _response.Data = data;
                }
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> UpdateDealerStatus(DealerStatusUpdate Request)
        {
            int result = await _dealerService.UpdateDealerStatus(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result > 0)
            {
                _response.IsSuccess = true;
                _response.Message = "Dealer Status Update Sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadDealerTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.DealerImportFormatFileName));
            if (fileContent == null || fileContent.Length == 0)
            {
                _response.Message = ErrorConstants.FileNotExistsToDownload;
                _response.IsSuccess = false;
            }
            else
            {
                _response.Data = fileContent;
            }
            return _response;
        }
        #endregion

        #region DealerAddress
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDealeArddress(DealerAddressSaveParameters Request)
        {

            int result = await _dealerService.SaveDealerAddress(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Dealer Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Dealer details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDealerAddressList(DealerAddressSearchParameters request)
        {
            IEnumerable<DealerAddressDetailsResponse> lstDealerAddress = await _dealerService.GetDealerAddressList(request);

            _response.Data = lstDealerAddress.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDealerAddressDetails(long id)
        {
            DealerAddressDetailsResponse? dealerAddress;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                dealerAddress = await _dealerService.GetDealerAddressDetailsById(id);

                _response.Data = dealerAddress;

            }

            return _response;
        }
        #endregion

        #region DealerContactDetails
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDealerContactDetails(DealerContactDetailsSaveParameters Request)
        {
            int result = await _dealerService.SaveDealerContactDetails(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Dealer Contact is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Dealer Contact details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDealerContactDetailsList(DealerContactDetailsSearchParameters request)
        {
            IEnumerable<DealerContactDetailsResponse> lstDealerContactDetails = await _dealerService.GetDealerContactDetailsList(request);

            _response.Data = lstDealerContactDetails.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDealerContactDetails(long id)
        {
            DealerContactDetailsResponse? dealerContactDetails;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                dealerContactDetails = await _dealerService.GetDealerContactDetailsById(id);

                _response.Data = dealerContactDetails;

            }

            return _response;
        }

        #endregion
    }
}
