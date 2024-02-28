using CasaAPI.Models.Enums;
using CasaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models.Constants;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Data;
using System.Security.Policy;
using Microsoft.CodeAnalysis;

namespace CasaAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageProfileController : ControllerBase
    {
        private ResponseModel _response;
        private IAdminService _adminService;
        private IFileManager _fileManager;
        public ManageProfileController(IAdminService adminService, IFileManager fileManager)
        {
            _adminService = adminService;
            _fileManager = fileManager;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
        #region Role
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRole(RoleSaveParameters Request)
        {
            int result = await _adminService.SaveRole(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Role Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Role details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRoleList(RoleSearchParameters request)
        {
            IEnumerable<RoleDetailsResponse> lstRole = await _adminService.GetRoleList(request);
            _response.Data = lstRole.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetRoleDetails(long id)
        {
            RoleDetailsResponse? role;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                role = await _adminService.GetRoleDetailsById(id);
                _response.Data = role;
            }

            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadRoleTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.RoleImportFormatFileName));
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

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportRoleListToExcel(RoleSearchParameters request)
        {
            IEnumerable<RoleDetailsResponse> roleDetailsResponses;

            request.IsExport = true;
            roleDetailsResponses = await _adminService.GetRoleList(request);
            if (roleDetailsResponses != null && roleDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelRoleDataFile(roleDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportRoleData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<RoleImportSaveParameters> lstRoleImportDetails = new List<RoleImportSaveParameters>();
            List<RoleFailToImportValidationErrors>? lstRoleFailedToImport = new List<RoleFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            RoleImportSaveParameters tempRoleImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Role data";
                return _response;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                request.FileUpload.CopyTo(stream);
                using ExcelPackage package = new ExcelPackage(stream);
                currentSheet = package.Workbook.Worksheets;
                workSheet = currentSheet.First();
                noOfCol = workSheet.Dimension.End.Column;
                noOfRow = workSheet.Dimension.End.Row;

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "RoleName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempRoleImport = new RoleImportSaveParameters()
                    {
                        RoleName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempRoleImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstRoleImportDetails.Add(tempRoleImport);
                    }
                    else
                    {
                        lstRoleFailedToImport.Add(new RoleFailToImportValidationErrors()
                        {
                            RoleName = tempRoleImport.RoleName,
                            IsActive = tempRoleImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstRoleImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstRoleFailedToImport.AddRange(await _adminService.ImportRoleDetails(lstRoleImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Role list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstRoleFailedToImport != null && lstRoleFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidRoleDataFile(lstRoleFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidRoleDataFile(IEnumerable<RoleFailToImportValidationErrors> lstRoleFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Role_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "RoleName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (RoleFailToImportValidationErrors record in lstRoleFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.RoleName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }
        private byte[] GenerateExcelRoleDataFile(IEnumerable<RoleDetailsResponse> lstRoleToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Role_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Role");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Role Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (RoleDetailsResponse record in lstRoleToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.RoleName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();

                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }

        #endregion

        #region Reporting Hierarchy


        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetRoleHierarchyDetailsByRoleId(long roleId)
        {
            IEnumerable<SelectListResponse> lstResponse = await _adminService.GetRoleHierarchyDetailsByRoleId(roleId);
            _response.Data = lstResponse.ToList();
            return _response;
        }



        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveReportingHierarchy(ReportingHierarchySaveParameters Request)
        {
            int result = await _adminService.SaveReportingHierarchy(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Reporting Hierarchy is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Reporting Hierarchy details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetReportingHierarchyList(ReportingHierarchySearchParameters request)
        {
            IEnumerable<ReportingHierarchyDetailsResponse> lstRole = await _adminService.GetReportingHierarchyList(request);
            _response.Data = lstRole.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetReportingHierarchyDetails(long id)
        {
            ReportingHierarchyDetailsResponse? reportingRole;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                reportingRole = await _adminService.GetReportingHierarchyDetailsById(id);
                _response.Data = reportingRole;
            }

            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadReportingHierarchyTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.ReportingHierarchyImportFormatFileName));
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

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportReportingHierarchyListToExcel(ReportingHierarchySearchParameters request)
        {
            IEnumerable<ReportingHierarchyDetailsResponse> reportingHierarchyDetailsResponses;

            request.IsExport = true;
            reportingHierarchyDetailsResponses = await _adminService.GetReportingHierarchyList(request);
            if (reportingHierarchyDetailsResponses != null && reportingHierarchyDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelReportingHierarchyDataFile(reportingHierarchyDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportReportingHierarchyData([FromQuery] ImportRequest request)
        {           
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ReportingHierarchyImportSaveParameters> lstReportingHierarchyImportDetails = new List<ReportingHierarchyImportSaveParameters>();
            List<ReportingHierarchyFailToImportValidationErrors>? lstReportingHierarchysFailedToImport = new List<ReportingHierarchyFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            ReportingHierarchyImportSaveParameters tempReportingHierarchyImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Reporting Hierarchy data";
                return _response;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                request.FileUpload.CopyTo(stream);
                using ExcelPackage package = new ExcelPackage(stream);
                currentSheet = package.Workbook.Worksheets;
                workSheet = currentSheet.First();
                noOfCol = workSheet.Dimension.End.Column;
                noOfRow = workSheet.Dimension.End.Row;

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "RoleName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "ReportingRoleName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempReportingHierarchyImport = new ReportingHierarchyImportSaveParameters()
                    {
                        RoleName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        ReportingRoleName = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 3].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempReportingHierarchyImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstReportingHierarchyImportDetails.Add(tempReportingHierarchyImport);
                    }
                    else
                    {
                        lstReportingHierarchysFailedToImport.Add(new ReportingHierarchyFailToImportValidationErrors()
                        {
                            RoleName = tempReportingHierarchyImport.RoleName,
                            ReportingRoleName = tempReportingHierarchyImport.ReportingRoleName,
                            IsActive = tempReportingHierarchyImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstReportingHierarchyImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstReportingHierarchysFailedToImport.AddRange(await _adminService.ImportReportingHierarchyDetails(lstReportingHierarchyImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Reporting Hierarchy list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstReportingHierarchysFailedToImport != null && lstReportingHierarchysFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidReportingHierarchyDataFile(lstReportingHierarchysFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidReportingHierarchyDataFile(IEnumerable<ReportingHierarchyFailToImportValidationErrors> lstReportingHierarchyFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_ReportingHierarchy_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "RoleName";
                    WorkSheet1.Cells[1, 2].Value = "ReportingRoleName";
                    WorkSheet1.Cells[1, 3].Value = "IsActive";
                    WorkSheet1.Cells[1, 4].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (ReportingHierarchyFailToImportValidationErrors record in lstReportingHierarchyFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.RoleName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.ReportingRoleName;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }
        private byte[] GenerateExcelReportingHierarchyDataFile(IEnumerable<ReportingHierarchyDetailsResponse> lstReportingHierarchyToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"ReportingHierarchy_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("ReportingHierarchy");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Role Name";
                    excelWorksheet.Cells[1, 2].Value = "Reporting Role Name";
                    excelWorksheet.Cells[1, 3].Value = "Is Active?";

                    recordIndex = 3;

                    foreach (ReportingHierarchyDetailsResponse record in lstReportingHierarchyToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.RoleName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.ReportingRoleName;
                        excelWorksheet.Cells[recordIndex, 3].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                    excelWorksheet.Column(3).AutoFit();

                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }
        #endregion

        #region Employee
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveEmployee([FromForm] EmployeeSaveParameters Request)
        {            
            if (Request?.ImageUploadfiles?.Length > 0)
            {
                Request.ImageUploadURL = _fileManager.UploadProfilePicture(Request.ImageUploadfiles);
            }
            if (Request?.PhotoUploadfiles?.Length > 0)
            {
                Request.PhotoUploadURL = _fileManager.UploadProfilePicture(Request.PhotoUploadfiles);
            }
            if (Request?.UploadAddharCardfiles?.Length > 0)
            {
                Request.UploadAddharCardURL = _fileManager.UploadProfilePicture(Request.UploadAddharCardfiles);
            }
            int result = await _adminService.SaveEmployee(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Employee is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Employee details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetEmployeeList(EmployeeSearchParameters request)
        {
            IEnumerable<EmployeeDetailsResponse> lstEmployeeList = await _adminService.GetEmployeeList(request);

            List<EmployeeDetailsResponse> datalist = new List<EmployeeDetailsResponse>();
            if (lstEmployeeList != null && lstEmployeeList.ToList().Count > 0)
            {
                foreach (EmployeeDetailsResponse record in lstEmployeeList)
                {
                    EmployeeDetailsResponse data = new EmployeeDetailsResponse();
                    data.Id = record.Id;
        data.EmployeeName= record.EmployeeName;
        data.EmployeeCode= record.EmployeeCode;
        data.MobileNumber= record.MobileNumber;
        data.Email= record.Email;
        data.Department= record.Department;
        data.Role= record.Role;
        data.RoleName= record.RoleName;
        data.ReportingTo= record.ReportingTo;
        data.ReportingToName= record.ReportingToName;
        data.DateOfBirth= record.DateOfBirth;
        data.DateOfJoining= record.DateOfJoining;
        data.EmergencycontactNo= record.EmergencycontactNo;
        data.BloodGroup= record.BloodGroup;
        data.BloodGroupName= record.BloodGroupName;
        data.Gender= record.Gender;
        data.GenderName= record.GenderName;
        // data.MaterialStatus= record.Id;
        data.CompanyNaumber= record.CompanyNaumber;
        data.PermanentAddress= record.PermanentAddress;
        data.PermanentState= record.PermanentState;
        data.PermanentRegion= record.PermanentRegion;
        data.PermanentDistrict= record.PermanentDistrict;
        data.PermanentCity= record.PermanentCity;
        data.PermanentArea= record.PermanentArea;
        data.PermanentStateName= record.PermanentStateName;
        data.PermanentRegionName= record.PermanentRegionName;
        data.PermanentDistrictName= record.PermanentDistrictName;
        data.PermanentCityName= record.PermanentCityName;
        data.PermanentAreaName= record.PermanentAreaName;
        data.PermanentPinCode= record.PermanentPinCode;
        data.IsTemporaryAddressIsSame= record.IsTemporaryAddressIsSame;
        data.TemporaryAddress= record.TemporaryAddress;
        data.TemporaryState= record.TemporaryState;
        data.TemporaryRegion= record.TemporaryRegion;
        data.TemporaryDistrict= record.TemporaryDistrict;
        data.TemporaryCity= record.TemporaryCity;
        data.TemporaryArea= record.TemporaryArea;
        data.TemporaryStateName= record.TemporaryStateName;
        data.TemporaryRegionName= record.TemporaryRegionName;
        data.TemporaryDistrictName= record.TemporaryDistrictName;
        data.TemporaryCityName= record.TemporaryCityName;
        data.TemporaryAreaName= record.TemporaryAreaName;
        data.TemporaryPinCode= record.TemporaryPinCode;
        data.EmergencyName= record.EmergencyName;
        data.EmergencyNumber= record.EmergencyNumber;
        data.EmergencyRelation= record.EmergencyRelation;
                    data.EmployeePostCompanyName = record.EmployeePostCompanyName;
        data.TotalNumberOfExp= record.TotalNumberOfExp;
        data.AddharNumber= record.AddharNumber;
                    if (record?.UploadAddharCard?.Length > 0)
                    {
                        data.UploadAddharCard = Convert.ToBase64String(_fileManager.GetProfilePicture(record?.UploadAddharCard));
                    }
        data.PANNumber= record.PANNumber;
        data.OtherProof= record.OtherProof;
                    if (record?.UploadAddharCard?.Length > 0)
                    {
                        data.PhotoUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(record?.PhotoUpload));
                    }
        data.Remark= record.Remark;
        data.IsWebUser= record.IsWebUser;
        data.IsMobileUser= record.IsMobileUser;
                    if (record?.UploadAddharCard?.Length > 0)
                    {
                        data.ImageUpload= Convert.ToBase64String(_fileManager.GetProfilePicture(record?.ImageUpload)); 
                    }
                    data.CreatorName = record.CreatorName;
                    data.CreatedOn = record.CreatedOn;
                    data.IsActive = record.IsActive;
                    datalist.Add(data);
                }
            }
            _response.Data = datalist.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetEmployeeDetails(long id)
        {
            EmployeeDetailsResponse? employee;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                employee = await _adminService.GetEmployeeDetailsById(id);
                if (employee != null)
                {
                    EmployeeDetailsResponse data = new EmployeeDetailsResponse();
                    data.Id = employee.Id;
                    data.EmployeeName = employee.EmployeeName;
                    data.EmployeeCode = employee.EmployeeCode;
                    data.MobileNumber = employee.MobileNumber;
                    data.Email = employee.Email;
                    data.Department = employee.Department;
                    data.Role = employee.Role;
                    data.RoleName = employee.RoleName;
                    data.ReportingTo = employee.ReportingTo;
                    data.ReportingToName = employee.ReportingToName;
                    data.DateOfBirth = employee.DateOfBirth;
                    data.DateOfJoining = employee.DateOfJoining;
                    data.EmergencycontactNo = employee.EmergencycontactNo;
                    data.BloodGroup = employee.BloodGroup;
                    data.BloodGroupName = employee.BloodGroupName;
                    data.Gender = employee.Gender;
                    data.GenderName = employee.GenderName;
                    data.CompanyNaumber = employee.CompanyNaumber;
                    data.PermanentAddress = employee.PermanentAddress;
                    data.PermanentState = employee.PermanentState;
                    data.PermanentRegion = employee.PermanentRegion;
                    data.PermanentDistrict = employee.PermanentDistrict;
                    data.PermanentCity = employee.PermanentCity;
                    data.PermanentArea = employee.PermanentArea;
                    data.PermanentStateName = employee.PermanentStateName;
                    data.PermanentRegionName = employee.PermanentRegionName;
                    data.PermanentDistrictName = employee.PermanentDistrictName;
                    data.PermanentCityName = employee.PermanentCityName;
                    data.PermanentAreaName = employee.PermanentAreaName;
                    data.PermanentPinCode = employee.PermanentPinCode;
                    data.IsTemporaryAddressIsSame = employee.IsTemporaryAddressIsSame;
                    data.TemporaryAddress = employee.TemporaryAddress;
                    data.TemporaryState = employee.TemporaryState;
                    data.TemporaryRegion = employee.TemporaryRegion;
                    data.TemporaryDistrict = employee.TemporaryDistrict;
                    data.TemporaryCity = employee.TemporaryCity;
                    data.TemporaryArea = employee.TemporaryArea;
                    data.TemporaryStateName = employee.TemporaryStateName;
                    data.TemporaryRegionName = employee.TemporaryRegionName;
                    data.TemporaryDistrictName = employee.TemporaryDistrictName;
                    data.TemporaryCityName = employee.TemporaryCityName;
                    data.TemporaryAreaName = employee.TemporaryAreaName;
                    data.TemporaryPinCode = employee.TemporaryPinCode;
                    data.EmergencyName = employee.EmergencyName;
                    data.EmergencyNumber = employee.EmergencyNumber;
                    data.EmergencyRelation = employee.EmergencyRelation;
                    data.EmployeePostCompanyName = employee.EmployeePostCompanyName;
                    data.TotalNumberOfExp = employee.TotalNumberOfExp;
                    data.AddharNumber = employee.AddharNumber;
                    if (employee?.UploadAddharCard?.Length > 0)
                    {
                        data.UploadAddharCard = Convert.ToBase64String(_fileManager.GetProfilePicture(employee.UploadAddharCard));
                    }
                    data.PANNumber = employee.PANNumber;
                    data.OtherProof = employee.OtherProof;
                    if (employee?.UploadAddharCard?.Length > 0)
                    {
                        data.PhotoUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(employee.PhotoUpload));
                    }
                    data.Remark = employee.Remark;
                    data.IsWebUser = employee.IsWebUser;
                    data.IsMobileUser = employee.IsMobileUser;
                    if (employee?.UploadAddharCard?.Length > 0)
                    {
                        data.ImageUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(employee.ImageUpload));
                    }
                    data.CreatorName = employee.CreatorName;
                    data.CreatedOn = employee.CreatedOn;
                    data.IsActive = employee.IsActive;
                    _response.Data = data;
                }
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetEmployeeListByRoleId(long roleId)
        {
            IEnumerable<EmployeeDetailsResponse> lstEmployeeList = await _adminService.GetEmployeeListByRoleId(roleId);

            List<EmployeeDetailsResponse> datalist = new List<EmployeeDetailsResponse>();
            if (lstEmployeeList != null && lstEmployeeList.ToList().Count > 0)
            {
                foreach (EmployeeDetailsResponse record in lstEmployeeList)
                {
                    EmployeeDetailsResponse data = new EmployeeDetailsResponse();
                    data.Id = record.Id;
                    data.EmployeeName = record.EmployeeName;
                    data.EmployeeCode = record.EmployeeCode;
                    data.MobileNumber = record.MobileNumber;
                    data.Email = record.Email;
                    data.Department = record.Department;
                    data.Role = record.Role;
                    data.RoleName = record.RoleName;
                    data.ReportingTo = record.ReportingTo;
                    data.ReportingToName = record.ReportingToName;
                    data.DateOfBirth = record.DateOfBirth;
                    data.DateOfJoining = record.DateOfJoining;
                    data.EmergencycontactNo = record.EmergencycontactNo;
                    data.BloodGroup = record.BloodGroup;
                    data.BloodGroupName = record.BloodGroupName;
                    data.Gender = record.Gender;
                    data.GenderName = record.GenderName;
                    // data.MaterialStatus= record.Id;
                    data.CompanyNaumber = record.CompanyNaumber;
                    data.PermanentAddress = record.PermanentAddress;
                    data.PermanentState = record.PermanentState;
                    data.PermanentRegion = record.PermanentRegion;
                    data.PermanentDistrict = record.PermanentDistrict;
                    data.PermanentCity = record.PermanentCity;
                    data.PermanentArea = record.PermanentArea;
                    data.PermanentStateName = record.PermanentStateName;
                    data.PermanentRegionName = record.PermanentRegionName;
                    data.PermanentDistrictName = record.PermanentDistrictName;
                    data.PermanentCityName = record.PermanentCityName;
                    data.PermanentAreaName = record.PermanentAreaName;
                    data.PermanentPinCode = record.PermanentPinCode;
                    data.IsTemporaryAddressIsSame = record.IsTemporaryAddressIsSame;
                    data.TemporaryAddress = record.TemporaryAddress;
                    data.TemporaryState = record.TemporaryState;
                    data.TemporaryRegion = record.TemporaryRegion;
                    data.TemporaryDistrict = record.TemporaryDistrict;
                    data.TemporaryCity = record.TemporaryCity;
                    data.TemporaryArea = record.TemporaryArea;
                    data.TemporaryStateName = record.TemporaryStateName;
                    data.TemporaryRegionName = record.TemporaryRegionName;
                    data.TemporaryDistrictName = record.TemporaryDistrictName;
                    data.TemporaryCityName = record.TemporaryCityName;
                    data.TemporaryAreaName = record.TemporaryAreaName;
                    data.TemporaryPinCode = record.TemporaryPinCode;
                    data.EmergencyName = record.EmergencyName;
                    data.EmergencyNumber = record.EmergencyNumber;
                    data.EmergencyRelation = record.EmergencyRelation;
                    data.EmployeePostCompanyName = record.EmployeePostCompanyName;
                    data.TotalNumberOfExp = record.TotalNumberOfExp;
                    data.AddharNumber = record.AddharNumber;
                    if (record?.UploadAddharCard?.Length > 0)
                    {
                        data.UploadAddharCard = Convert.ToBase64String(_fileManager.GetProfilePicture(record?.UploadAddharCard));
                    }
                    data.PANNumber = record.PANNumber;
                    data.OtherProof = record.OtherProof;
                    if (record?.UploadAddharCard?.Length > 0)
                    {
                        data.PhotoUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(record?.PhotoUpload));
                    }
                    data.Remark = record.Remark;
                    data.IsWebUser = record.IsWebUser;
                    data.IsMobileUser = record.IsMobileUser;
                    if (record?.UploadAddharCard?.Length > 0)
                    {
                        data.ImageUpload = Convert.ToBase64String(_fileManager.GetProfilePicture(record?.ImageUpload));
                    }
                    data.CreatorName = record.CreatorName;
                    data.CreatedOn = record.CreatedOn;
                    data.IsActive = record.IsActive;
                    datalist.Add(data);
                }
            }
            _response.Data = datalist.ToList();
            return _response;
        }


        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadEmployeeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.EmployeeImportFormatFileName));
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

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportEmployeeListToExcel(EmployeeSearchParameters request)
        {
            IEnumerable<EmployeeDetailsResponse> employeeDetailsResponses;

            request.IsExport = true;
            employeeDetailsResponses = await _adminService.GetEmployeeList(request);
            if (employeeDetailsResponses != null && employeeDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelEmployeeDataFile(employeeDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelEmployeeDataFile(IEnumerable<EmployeeDetailsResponse> lstEmployeeToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Employee_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Employee");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;


                    excelWorksheet.Cells[1, 1].Value = "Id";
                    excelWorksheet.Cells[1, 2].Value = "EmployeeName";
                    excelWorksheet.Cells[1, 3].Value = "EmployeeCode";
                    excelWorksheet.Cells[1, 4].Value = "MobileNumber";
                    excelWorksheet.Cells[1, 5].Value = "Email";
                    excelWorksheet.Cells[1, 6].Value = "Department";
                    excelWorksheet.Cells[1, 7].Value = "Role";
                    excelWorksheet.Cells[1, 8].Value = "ReportingTo";
                    excelWorksheet.Cells[1, 9].Value = "DateOfBirth";
                    excelWorksheet.Cells[1, 10].Value = "DateOfJoining ";
                    excelWorksheet.Cells[1, 11].Value = "EmergencycontactNo";
                    excelWorksheet.Cells[1, 12].Value = "BloodGroup ";
                    excelWorksheet.Cells[1, 13].Value = "Gender ";
                    excelWorksheet.Cells[1, 14].Value = "MaterialStatus ";
                    excelWorksheet.Cells[1, 15].Value = "CompanyNaumber";
                    excelWorksheet.Cells[1, 16].Value = "PermanentAddress";
                    excelWorksheet.Cells[1, 17].Value = "PermanentState ";
                    excelWorksheet.Cells[1, 18].Value = "PermanentRegion ";
                    excelWorksheet.Cells[1, 19].Value = "PermanentDistrict ";
                    excelWorksheet.Cells[1, 20].Value = "PermanentCity ";
                    excelWorksheet.Cells[1, 21].Value = "PermanentArea ";
                    excelWorksheet.Cells[1, 1].Value = "PermanentPinCode";
                    excelWorksheet.Cells[1, 22].Value = "IsTemporaryAddressIsSame";
                    excelWorksheet.Cells[1, 23].Value = "TemporaryAddress";
                    excelWorksheet.Cells[1, 24].Value = "TemporaryState ";
                    excelWorksheet.Cells[1, 25].Value = "TemporaryRegion ";
                    excelWorksheet.Cells[1, 26].Value = "TemporaryDistrict ";
                    excelWorksheet.Cells[1, 27].Value = "TemporaryCity ";
                    excelWorksheet.Cells[1, 28].Value = "TemporaryArea ";
                    excelWorksheet.Cells[1, 29].Value = "TemporaryPinCode";
                    excelWorksheet.Cells[1, 30].Value = "EmergencyName";
                    excelWorksheet.Cells[1, 31].Value = "EmergencyNumber";
                    excelWorksheet.Cells[1, 32].Value = "EmergencyRelation";
                    excelWorksheet.Cells[1, 33].Value = "EmployeePostCompanyName";
                    excelWorksheet.Cells[1, 34].Value = "TotalNumberOfExp";
                    excelWorksheet.Cells[1, 35].Value = "AddharNumber";
                    excelWorksheet.Cells[1, 36].Value = "UploadAddharCard";
                    excelWorksheet.Cells[1, 37].Value = "PANNumber";
                    excelWorksheet.Cells[1, 38].Value = "OtherProof";
                    excelWorksheet.Cells[1, 39].Value = "PhotoUpload";
                    excelWorksheet.Cells[1, 40].Value = "Remark";
                    excelWorksheet.Cells[1, 41].Value = "IsWebUser";
                    excelWorksheet.Cells[1, 42].Value = "IsMobileUser";
                    excelWorksheet.Cells[1, 43].Value = "ImageUpload";
                    excelWorksheet.Cells[1, 44].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (EmployeeDetailsResponse record in lstEmployeeToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.Id;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.EmployeeName;
                        excelWorksheet.Cells[recordIndex, 3].Value = record.EmployeeCode;
                        excelWorksheet.Cells[recordIndex, 4].Value = record.MobileNumber;
                        excelWorksheet.Cells[recordIndex, 5].Value = record.Email;
                        excelWorksheet.Cells[recordIndex, 6].Value = record.Department;
                        excelWorksheet.Cells[recordIndex, 7].Value = record.Role;
                        excelWorksheet.Cells[recordIndex, 8].Value = record.ReportingTo;
                        excelWorksheet.Cells[recordIndex, 9].Value = record.DateOfBirth;
                        excelWorksheet.Cells[recordIndex, 10].Value = record.DateOfJoining;
                        excelWorksheet.Cells[recordIndex, 11].Value = record.EmergencycontactNo;
                        excelWorksheet.Cells[recordIndex, 12].Value = record.BloodGroup;
                        excelWorksheet.Cells[recordIndex, 13].Value = record.Gender;
                       // excelWorksheet.Cells[recordIndex, 14].Value = record.MaterialStatus;
                        excelWorksheet.Cells[recordIndex, 15].Value = record.CompanyNaumber;
                        excelWorksheet.Cells[recordIndex, 16].Value = record.PermanentAddress;
                        excelWorksheet.Cells[recordIndex, 17].Value = record.PermanentState;
                        excelWorksheet.Cells[recordIndex, 18].Value = record.PermanentRegion;
                        excelWorksheet.Cells[recordIndex, 19].Value = record.PermanentDistrict;
                        excelWorksheet.Cells[recordIndex, 20].Value = record.PermanentCity;
                        excelWorksheet.Cells[recordIndex, 21].Value = record.PermanentArea;
                        excelWorksheet.Cells[recordIndex, 1].Value = record.PermanentPinCode;
                        excelWorksheet.Cells[recordIndex, 22].Value = record.IsTemporaryAddressIsSame;
                        excelWorksheet.Cells[recordIndex, 23].Value = record.TemporaryAddress;
                        excelWorksheet.Cells[recordIndex, 24].Value = record.TemporaryState;
                        excelWorksheet.Cells[recordIndex, 25].Value = record.TemporaryRegion;
                        excelWorksheet.Cells[recordIndex, 26].Value = record.TemporaryDistrict;
                        excelWorksheet.Cells[recordIndex, 27].Value = record.TemporaryCity;
                        excelWorksheet.Cells[recordIndex, 28].Value = record.TemporaryArea;
                        excelWorksheet.Cells[recordIndex, 29].Value = record.TemporaryPinCode;
                        excelWorksheet.Cells[recordIndex, 30].Value = record.EmergencyName;
                        excelWorksheet.Cells[recordIndex, 31].Value = record.EmergencyNumber;
                        excelWorksheet.Cells[recordIndex, 32].Value = record.EmergencyRelation;
                        excelWorksheet.Cells[recordIndex, 33].Value = record.EmployeePostCompanyName;
                        excelWorksheet.Cells[recordIndex, 34].Value = record.TotalNumberOfExp;
                        excelWorksheet.Cells[recordIndex, 35].Value = record.AddharNumber;
                        excelWorksheet.Cells[recordIndex, 36].Value = record.UploadAddharCard;
                        excelWorksheet.Cells[recordIndex, 37].Value = record.PANNumber;
                        excelWorksheet.Cells[recordIndex, 38].Value = record.OtherProof;
                        excelWorksheet.Cells[recordIndex, 39].Value = record.PhotoUpload;
                        excelWorksheet.Cells[recordIndex, 40].Value = record.Remark;
                        excelWorksheet.Cells[recordIndex, 41].Value = record.IsWebUser;
                        excelWorksheet.Cells[recordIndex, 42].Value = record.IsMobileUser;
                        excelWorksheet.Cells[recordIndex, 43].Value = record.ImageUpload;
                        excelWorksheet.Cells[recordIndex, 44].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                    excelWorksheet.Column(3).AutoFit();
                    excelWorksheet.Column(4).AutoFit();
                    excelWorksheet.Column(5).AutoFit();
                    excelWorksheet.Column(6).AutoFit();
                    excelWorksheet.Column(7).AutoFit();
                    excelWorksheet.Column(8).AutoFit();
                    excelWorksheet.Column(9).AutoFit();
                    excelWorksheet.Column(10).AutoFit();
                    excelWorksheet.Column(11).AutoFit();
                    excelWorksheet.Column(12).AutoFit();
                    excelWorksheet.Column(13).AutoFit();
                    excelWorksheet.Column(14).AutoFit();
                    excelWorksheet.Column(15).AutoFit();
                    excelWorksheet.Column(16).AutoFit();
                    excelWorksheet.Column(17).AutoFit();
                    excelWorksheet.Column(18).AutoFit();
                    excelWorksheet.Column(19).AutoFit();
                    excelWorksheet.Column(20).AutoFit();
                    excelWorksheet.Column(21).AutoFit();
                    excelWorksheet.Column(22).AutoFit();
                    excelWorksheet.Column(23).AutoFit();
                    excelWorksheet.Column(24).AutoFit();
                    excelWorksheet.Column(25).AutoFit();
                    excelWorksheet.Column(26).AutoFit();
                    excelWorksheet.Column(27).AutoFit();
                    excelWorksheet.Column(28).AutoFit();
                    excelWorksheet.Column(29).AutoFit();
                    excelWorksheet.Column(30).AutoFit();
                    excelWorksheet.Column(31).AutoFit();
                    excelWorksheet.Column(32).AutoFit();
                    excelWorksheet.Column(33).AutoFit();
                    excelWorksheet.Column(34).AutoFit();
                    excelWorksheet.Column(35).AutoFit();
                    excelWorksheet.Column(36).AutoFit();
                    excelWorksheet.Column(37).AutoFit();
                    excelWorksheet.Column(38).AutoFit();
                    excelWorksheet.Column(39).AutoFit();
                    excelWorksheet.Column(40).AutoFit();
                    excelWorksheet.Column(41).AutoFit();
                    excelWorksheet.Column(42).AutoFit();
                    excelWorksheet.Column(43).AutoFit();
                    excelWorksheet.Column(444).AutoFit();
                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }

        #endregion

    }
}
