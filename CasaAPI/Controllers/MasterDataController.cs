using CasaAPI.Controllers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using Microsoft.AspNetCore.Mvc;

namespace AVSalesBoosterAPI.Controllers
{
    public class MasterDataController : CustomBaseController
    {
        private ResponseModel _response;
        private IAdminService _adminService;

        public MasterDataController(IAdminService adminService)
        {
            _adminService = adminService;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
       

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSizeForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetSizeForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBrandForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetBrandForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCollectionForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetCollectionForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCategoryForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetCategoryForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetTypeForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPunchForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetPunchForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSurfaceForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetSurfaceForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetThicknessForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetThicknessForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTileForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetTileForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }
     
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSubVendorForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetSubVendorForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }
       
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContactTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetContactTypeForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }
      
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetReferralForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetReferralForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCustomerTypesForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetCustomerTypesForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCustomersForSelectList(CustomerSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetCustomersForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetStatusMasterForSelectList()
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetStatusMasterForSelectList(StatusTypeCode.Common);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetLeaveStatusListForSelectList()
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetStatusMasterForSelectList(StatusTypeCode.LeaveTypes);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetReportingToEmpListForSelectList(ReportingToEmpListParameters parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetReportingToEmployeeForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCustomerContactsListForFields(CustomerContactsListRequest parameters)
        {
            IEnumerable<CustomerContactsListForFields> lstResponse = await _adminService.GetCustomerContactsListForFields(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }
    }
}
