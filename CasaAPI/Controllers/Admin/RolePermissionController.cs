using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Data.Common;
using System.Data.SqlClient;
using Models;

namespace CasaAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionController : ControllerBase
    {
        private ResponseModel _response;
        private IAdminService _adminService;
        private IFileManager _fileManager;
        public RolePermissionController(IAdminService adminService, IFileManager fileManager)
        {
            _adminService = adminService;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Role
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePage(PageSaveParameters Request)
        {
            int result = await _adminService.SavePage(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Page Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Page details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> UpdateRolePermission(List<RolePermissionUpdateParameters> rolePermission)
        {
           
                    int result = await _adminService.UpdateRolePermission(rolePermission);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Role Permission is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Role Permission details Update sucessfully";
            }
            return _response;
        } 

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPageList(PageSearchParameters request)
        {
            IEnumerable<PageDetailsResponse> lstPage = await _adminService.GetPageList(request);
            _response.Data = lstPage.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPageDetails(long id)
        {
            PageDetailsResponse? page;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                page = await _adminService.GetPageDetailsById(id);
                _response.Data = page;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetRollPermissionDetails(long roleId)
        {
                IEnumerable<RollPermissionDetailsResponse> lstPage = await _adminService.GetRollPermissionDetailsByRoleId(roleId);
                _response.Data = lstPage.ToList();            
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRollPermissionList(RollPermissionSearchParameters request)
        {
            IEnumerable<RollPermissionDetailsResponse> lstPage = await _adminService.GetRollPermissionList(request);
            _response.Data = lstPage.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> UpdateEmployeePermission(List<EmployeePermissionUpdateParameters> employeePermission)
        {

            int result = await _adminService.UpdateEmployeePermission(employeePermission);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Employee Permission is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Employee Permission details Update sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetEmployeePermissionDetails(long employeeId)
        {
            IEnumerable<EmployeePermissionDetailsResponse> lstPage = await _adminService.GetEmployeePermissionDetailsByEmployeeId(employeeId);
            _response.Data = lstPage.ToList();            
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetEmployeePermissionList(EmployeePermissionSearchParameters request)
        {
            IEnumerable<EmployeePermissionDetailsResponse> lstPage = await _adminService.GetEmployeePermissionList(request);
            _response.Data = lstPage.ToList();
            return _response;
        }

        #endregion
    }
}
