using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using CasaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using static CasaAPI.Models.VendorModel;
using static CasaAPI.Models.SubVendorModel;
using static CasaAPI.Models.CompanyTypeModel;
using static CasaAPI.Models.VendorGroupModel;
using static CasaAPI.Models.TDSNatureModel;
using static CasaAPI.Models.AllVendorModel;

namespace CasaAPI.Controllers.Admin
{
    public class ManageVendorController : CustomBaseController
    {
        private ResponseModel _response;
        private IVendorService _vendorService;
        private IFileManager _fileManager;
        public ManageVendorController(IVendorService vendorService, IFileManager fileManager)
        {
            _vendorService = vendorService;
            _fileManager = fileManager;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
        #region Vendor
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVendor(VendorSaveParameters Request)
        {
            int result = await _vendorService.SaveVendor(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Vendor Group is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Vendor Group details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVendorList(VendorSearchParameters request)
        {
            IEnumerable<VendorDetailsResponse> lstVendors = await _vendorService.GetVendorsList(request);
            _response.Data = lstVendors.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVendorForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _vendorService.GetVendorForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetVendorDetails(long id)
        {
            VendorDetailsResponse? Vendor;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                Vendor = await _vendorService.GetVendorDetailsById(id);
                _response.Data = Vendor;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportVendorsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<VendorImportSaveParameters> lstVendorImportDetails = new List<VendorImportSaveParameters>();
            List<VendorFailToImportValidationErrors>? lstVendorsFailedToImport = new List<VendorFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            VendorImportSaveParameters tempVendorImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Vendor data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "VendorType", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempVendorImport = new VendorImportSaveParameters()
                    {
                        VendorType = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempVendorImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstVendorImportDetails.Add(tempVendorImport);
                    }
                    else
                    {
                        lstVendorsFailedToImport.Add(new VendorFailToImportValidationErrors()
                        {
                            VendorType = tempVendorImport.VendorType,
                            IsActive = tempVendorImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstVendorImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstVendorsFailedToImport.AddRange(await _vendorService.ImportVendorsDetails(lstVendorImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Vendors list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstVendorsFailedToImport != null && lstVendorsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidVendorDataFile(lstVendorsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidVendorDataFile(IEnumerable<VendorFailToImportValidationErrors> lstVendorsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Vendor_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "VendorType";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (VendorFailToImportValidationErrors record in lstVendorsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.VendorType;
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

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadVendorTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.VendorImportFormatFileName));
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
        public async Task<ResponseModel> ExportVendorListToExcel(VendorSearchParameters request)
        {
            IEnumerable<VendorDetailsResponse> VendorDetailsResponses;

            request.IsExport = true;
            VendorDetailsResponses = await _vendorService.GetVendorsList(request);
            if (VendorDetailsResponses != null && VendorDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelVendorDataFile(VendorDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelVendorDataFile(IEnumerable<VendorDetailsResponse> lstVendorsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Vendor_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Vendor");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Vendor Group";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (VendorDetailsResponse record in lstVendorsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.VendorType;
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
        ///
        #region Sub Vendor
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveSubVendor(SubVendorSaveParameters Request)
        {
            int result = await _vendorService.SaveSubVendor(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Vendor Group is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Sub Vendor details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSubVendorList(SubVendorSearchParameters request)
        {
            IEnumerable<SubVendorDetailsResponse> lstSubVendors = await _vendorService.GetSubVendorsList(request);
            _response.Data = lstSubVendors.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetSubVendorDetails(long id)
        {
            SubVendorDetailsResponse? SubVendor;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                SubVendor = await _vendorService.GetSubVendorDetailsById(id);
                _response.Data = SubVendor;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportSubVendorsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<SubVendorImportSaveParameters> lstSubVendorImportDetails = new List<SubVendorImportSaveParameters>();
            List<SubVendorFailToImportValidationErrors>? lstSubVendorsFailedToImport = new List<SubVendorFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            SubVendorImportSaveParameters tempSubVendorImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import SubVendor data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "SubVendorType", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempSubVendorImport = new SubVendorImportSaveParameters()
                    {
                        SubVendorType = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempSubVendorImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstSubVendorImportDetails.Add(tempSubVendorImport);
                    }
                    else
                    {
                        lstSubVendorsFailedToImport.Add(new SubVendorFailToImportValidationErrors()
                        {
                            SubVendorType = tempSubVendorImport.SubVendorType,
                            IsActive = tempSubVendorImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstSubVendorImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstSubVendorsFailedToImport.AddRange(await _vendorService.ImportSubVendorsDetails(lstSubVendorImportDetails));

            _response.IsSuccess = true;
            _response.Message = "SubVendors list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstSubVendorsFailedToImport != null && lstSubVendorsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidSubVendorDataFile(lstSubVendorsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidSubVendorDataFile(IEnumerable<SubVendorFailToImportValidationErrors> lstSubVendorsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_SubVendor_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "SubVendorType";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (SubVendorFailToImportValidationErrors record in lstSubVendorsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.SubVendorType;
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

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadSubVendorTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.SubVendorImportFormatFileName));
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
        public async Task<ResponseModel> ExportSubVendorListToExcel(SubVendorSearchParameters request)
        {
            IEnumerable<SubVendorDetailsResponse> SubVendorDetailsResponses;

            request.IsExport = true;
            SubVendorDetailsResponses = await _vendorService.GetSubVendorsList(request);
            if (SubVendorDetailsResponses != null && SubVendorDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelSubVendorDataFile(SubVendorDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelSubVendorDataFile(IEnumerable<SubVendorDetailsResponse> lstSubVendorsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"SubVendor_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("SubVendor");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "SubVendor Group";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (SubVendorDetailsResponse record in lstSubVendorsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.SubVendorType;
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

        #region Company Type
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCompanyType(CompanyTypeSaveParameters Request)
        {
            int result = await _vendorService.SaveCompanyType(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Company Type is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Company Type details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCompanyTypeList(CompanyTypeSearchParameters request)
        {
            IEnumerable<CompanyTypeDetailsResponse> lstVendors = await _vendorService.GetCompanyTypesList(request);
            _response.Data = lstVendors.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCompanyTypeForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _vendorService.GetCompanyTypeForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCompanyTypeDetails(long id)
        {
            CompanyTypeDetailsResponse? CompanyType;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                CompanyType = await _vendorService.GetCompanyTypeDetailsById(id);
                _response.Data = CompanyType;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportCompanyTypesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<CompanyTypeImportSaveParameters> lstCompanyTypeImportDetails = new List<CompanyTypeImportSaveParameters>();
            List<CompanyTypeFailToImportValidationErrors>? lstCompanyTypesFailedToImport = new List<CompanyTypeFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            CompanyTypeImportSaveParameters tempCompanyTypeImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Company Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "CompanyType", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempCompanyTypeImport = new CompanyTypeImportSaveParameters()
                    {
                        CompanyType = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempCompanyTypeImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstCompanyTypeImportDetails.Add(tempCompanyTypeImport);
                    }
                    else
                    {
                        lstCompanyTypesFailedToImport.Add(new CompanyTypeFailToImportValidationErrors()
                        {
                            CompanyType = tempCompanyTypeImport.CompanyType,
                            IsActive = tempCompanyTypeImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstCompanyTypeImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstCompanyTypesFailedToImport.AddRange(await _vendorService.ImportCompanyTypesDetails(lstCompanyTypeImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Company Types list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstCompanyTypesFailedToImport != null && lstCompanyTypesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidCompanyTypeDataFile(lstCompanyTypesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidCompanyTypeDataFile(IEnumerable<CompanyTypeFailToImportValidationErrors> lstCompanyTypesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_CompanyType_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CompanyType";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (CompanyTypeFailToImportValidationErrors record in lstCompanyTypesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.CompanyType;
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

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadCompanyTypeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.CompanyTypeImportFormatFileName));
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
        public async Task<ResponseModel> ExportCompanyTypeListToExcel(CompanyTypeSearchParameters request)
        {
            IEnumerable<CompanyTypeDetailsResponse> CompanyTypeDetailsResponses;

            request.IsExport = true;
            CompanyTypeDetailsResponses = await _vendorService.GetCompanyTypesList(request);
            if (CompanyTypeDetailsResponses != null && CompanyTypeDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelCompanyTypeDataFile(CompanyTypeDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelCompanyTypeDataFile(IEnumerable<CompanyTypeDetailsResponse> lstCompanyTypesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"CompanyType_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("CompanyType");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Company Type";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (CompanyTypeDetailsResponse record in lstCompanyTypesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.CompanyType;
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

        #region Vendor Group
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVendorGroup(VendorGroupSaveParameters Request)
        {
            int result = await _vendorService.SaveVendorGroup(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Vendor Group is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Vendor Group details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVendorGroupList(VendorGroupSearchParameters request)
        {
            IEnumerable<VendorGroupDetailsResponse> lstVendors = await _vendorService.GetVendorGroupsList(request);
            _response.Data = lstVendors.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVendorGroupForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _vendorService.GetVendorGroupForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetVendorGroupDetails(long id)
        {
            VendorGroupDetailsResponse? VendorGroup;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                VendorGroup = await _vendorService.GetVendorGroupDetailsById(id);
                _response.Data = VendorGroup;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportVendorGroupsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<VendorGroupImportSaveParameters> lstVendorGroupImportDetails = new List<VendorGroupImportSaveParameters>();
            List<VendorGroupFailToImportValidationErrors>? lstVendorGroupsFailedToImport = new List<VendorGroupFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            VendorGroupImportSaveParameters tempVendorGroupImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Vendor Group data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "VendorGroup", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempVendorGroupImport = new VendorGroupImportSaveParameters()
                    {
                        VendorGroup = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempVendorGroupImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstVendorGroupImportDetails.Add(tempVendorGroupImport);
                    }
                    else
                    {
                        lstVendorGroupsFailedToImport.Add(new VendorGroupFailToImportValidationErrors()
                        {
                            VendorGroup = tempVendorGroupImport.VendorGroup,
                            IsActive = tempVendorGroupImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstVendorGroupImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstVendorGroupsFailedToImport.AddRange(await _vendorService.ImportVendorGroupsDetails(lstVendorGroupImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Vendor Groups list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstVendorGroupsFailedToImport != null && lstVendorGroupsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidVendorGroupDataFile(lstVendorGroupsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidVendorGroupDataFile(IEnumerable<VendorGroupFailToImportValidationErrors> lstVendorGroupsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_VendorGroup_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "VendorGroup";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (VendorGroupFailToImportValidationErrors record in lstVendorGroupsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.VendorGroup;
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

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadVendorGroupTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.VendorGroupImportFormatFileName));
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
        public async Task<ResponseModel> ExportVendorGroupListToExcel(VendorGroupSearchParameters request)
        {
            IEnumerable<VendorGroupDetailsResponse> VendorGroupDetailsResponses;

            request.IsExport = true;
            VendorGroupDetailsResponses = await _vendorService.GetVendorGroupsList(request);
            if (VendorGroupDetailsResponses != null && VendorGroupDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelVendorGroupDataFile(VendorGroupDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelVendorGroupDataFile(IEnumerable<VendorGroupDetailsResponse> lstVendorGroupsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"VendorGroup_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("VendorGroup");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Vendor Group";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (VendorGroupDetailsResponse record in lstVendorGroupsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.VendorGroup;
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

        #region TDS Nature
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveTDSNature(TDSNatureSaveParameters Request)
        {
            int result = await _vendorService.SaveTDSNature(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "TDS Nature is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "TDS Nature details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTDSNatureList(TDSNatureSearchParameters request)
        {
            IEnumerable<TDSNatureDetailsResponse> lstVendors = await _vendorService.GetTDSNaturesList(request);
            _response.Data = lstVendors.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTDSNatureForSelectList(CommonSelectListRequestModel parameters)
        {
            IEnumerable<SelectListResponse> lstResponse = await _vendorService.GetTDSNatureForSelectList(parameters);
            _response.Data = lstResponse.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetTDSNatureDetails(long id)
        {
            TDSNatureDetailsResponse? TDSNature;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                TDSNature = await _vendorService.GetTDSNatureDetailsById(id);
                _response.Data = TDSNature;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportTDSNaturesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<TDSNatureImportSaveParameters> lstTDSNatureImportDetails = new List<TDSNatureImportSaveParameters>();
            List<TDSNatureFailToImportValidationErrors>? lstTDSNaturesFailedToImport = new List<TDSNatureFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            TDSNatureImportSaveParameters tempTDSNatureImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Vendor Group data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "TDSNature", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempTDSNatureImport = new TDSNatureImportSaveParameters()
                    {
                        TDSNature = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempTDSNatureImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstTDSNatureImportDetails.Add(tempTDSNatureImport);
                    }
                    else
                    {
                        lstTDSNaturesFailedToImport.Add(new TDSNatureFailToImportValidationErrors()
                        {
                            TDSNature = tempTDSNatureImport.TDSNature,
                            IsActive = tempTDSNatureImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstTDSNatureImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstTDSNaturesFailedToImport.AddRange(await _vendorService.ImportTDSNaturesDetails(lstTDSNatureImportDetails));

            _response.IsSuccess = true;
            _response.Message = "TDS Natures list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstTDSNaturesFailedToImport != null && lstTDSNaturesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidTDSNatureDataFile(lstTDSNaturesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidTDSNatureDataFile(IEnumerable<TDSNatureFailToImportValidationErrors> lstTDSNaturesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_TDSNature_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "TDSNature";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (TDSNatureFailToImportValidationErrors record in lstTDSNaturesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.TDSNature;
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

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadTDSNatureTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.TDSNatureImportFormatFileName));
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
        public async Task<ResponseModel> ExportTDSNatureListToExcel(TDSNatureSearchParameters request)
        {
            IEnumerable<TDSNatureDetailsResponse> TDSNatureDetailsResponses;

            request.IsExport = true;
            TDSNatureDetailsResponses = await _vendorService.GetTDSNaturesList(request);
            if (TDSNatureDetailsResponses != null && TDSNatureDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelTDSNatureDataFile(TDSNatureDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelTDSNatureDataFile(IEnumerable<TDSNatureDetailsResponse> lstTDSNaturesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"TDSNature_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("TDSNature");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "TDS Nature";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (TDSNatureDetailsResponse record in lstTDSNaturesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.TDSNature;
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

        #region All Vender
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveAllVendor(AllVendorSaveParameters Request)
        {
            int result = await _vendorService.SaveAllVendor(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "TDS Nature is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "TDS Nature details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetAllVendorList(AllVendorSearchParameters request)
        {
            IEnumerable<AllVendorDetailsResponse> lstVendors = await _vendorService.GetAllVendorsList(request);
            _response.Data = lstVendors.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetAllVendorDetails(long id)
        {
            AllVendorDetailsResponse? AllVendor;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                AllVendor = await _vendorService.GetAllVendorDetailsById(id);
                _response.Data = AllVendor;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportAllVendorsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<AllVendorImportSaveParameters> lstAllVendorImportDetails = new List<AllVendorImportSaveParameters>();
            List<AllVendorFailToImportValidationErrors>? lstAllVendorsFailedToImport = new List<AllVendorFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            AllVendorImportSaveParameters tempAllVendorImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Vendor Group data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "Vendor", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 1].Value.ToString(), "SubVendor", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 1].Value.ToString(), "CompanyType", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 1].Value.ToString(), "VendorGroup", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempAllVendorImport = new AllVendorImportSaveParameters()
                    {
                        Vendor = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        SubVendor = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                        CompanyType = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                        VendorGroup = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 5].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempAllVendorImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstAllVendorImportDetails.Add(tempAllVendorImport);
                    }
                    else
                    {
                        lstAllVendorsFailedToImport.Add(new AllVendorFailToImportValidationErrors()
                        {
                            Vendor = tempAllVendorImport.Vendor,
                            SubVendor = tempAllVendorImport.SubVendor,
                            CompanyType = tempAllVendorImport.CompanyType,
                            VendorGroup = tempAllVendorImport.VendorGroup,
                            IsActive = tempAllVendorImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstAllVendorImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstAllVendorsFailedToImport.AddRange(await _vendorService.ImportAllVendorsDetails(lstAllVendorImportDetails));

            _response.IsSuccess = true;
            _response.Message = "TDS Natures list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstAllVendorsFailedToImport != null && lstAllVendorsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidAllVendorDataFile(lstAllVendorsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidAllVendorDataFile(IEnumerable<AllVendorFailToImportValidationErrors> lstAllVendorsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_AllVendor_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Vendor";
                    WorkSheet1.Cells[1, 2].Value = "SubVendor";
                    WorkSheet1.Cells[1, 3].Value = "CompanyType";
                    WorkSheet1.Cells[1, 4].Value = "VendorGroup";
                    WorkSheet1.Cells[1, 5].Value = "IsActive";
                    WorkSheet1.Cells[1, 6].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (AllVendorFailToImportValidationErrors record in lstAllVendorsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.Vendor;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.SubVendor;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.CompanyType;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.VendorGroup;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 6].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();
                    WorkSheet1.Column(5).AutoFit();
                    WorkSheet1.Column(6).AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadAllVendorTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.AllVendorImportFormatFileName));
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
        public async Task<ResponseModel> ExportAllVendorListToExcel(AllVendorSearchParameters request)
        {
            IEnumerable<AllVendorDetailsResponse> AllVendorDetailsResponses;

            request.IsExport = true;
            AllVendorDetailsResponses = await _vendorService.GetAllVendorsList(request);
            if (AllVendorDetailsResponses != null && AllVendorDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelAllVendorDataFile(AllVendorDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelAllVendorDataFile(IEnumerable<AllVendorDetailsResponse> lstAllVendorsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"AllVendor_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("AllVendor");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Vendor Type";
                    excelWorksheet.Cells[1, 2].Value = "Sub Vendor";
                    excelWorksheet.Cells[1, 3].Value = "Company Type";
                    excelWorksheet.Cells[1, 4].Value = "Vendor Group";
                    excelWorksheet.Cells[1, 5].Value = "Is Active?";

                    recordIndex = 5;

                    foreach (AllVendorDetailsResponse record in lstAllVendorsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.VendorType;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.SubVendor;
                        excelWorksheet.Cells[recordIndex, 3].Value = record.CompanyType;
                        excelWorksheet.Cells[recordIndex, 4].Value = record.VendorGroup;
                        excelWorksheet.Cells[recordIndex, 5].Value = record.IsActive;                        
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                    excelWorksheet.Column(3).AutoFit();
                    excelWorksheet.Column(4).AutoFit();
                    excelWorksheet.Column(5).AutoFit();
                  
                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }

        #endregion

        //#region ManageVender
        //[Route("[action]")]
        //[HttpPost]
        //public async Task<ResponseModel> SaveManageVender(VenderSaveParameters Request)
        //{
        //    int result = await _vendorService.SaveVender(Request);
        //    _response.IsSuccess = false;

        //    if (result == (int)SaveEnums.NoRecordExists)
        //    {
        //        _response.Message = "No record exists";
        //    }
        //    else if (result == (int)SaveEnums.NameExists)
        //    {
        //        _response.Message = "Vender Name is already exists";
        //    }
        //    else if (result == (int)SaveEnums.NoResult)
        //    {
        //        _response.Message = "Something went wrong, please try again";
        //    }
        //    else
        //    {
        //        _response.IsSuccess = true;
        //        _response.Message = "Vsender details saved sucessfully";
        //    }
        //    return _response;
        //}

        //[Route("[action]")]
        //[HttpPost]
        //public async Task<ResponseModel> GetManageVenderList(VenderSearchParameters request)
        //{
        //    IEnumerable<VenderDetailsResponse> lstDealer = await _vendorService.GetVenderList(request);
        //    _response.Data = lstDealer.ToList();
        //    return _response;
        //}

        //[Route("[action]")]
        //[HttpGet]
        //public async Task<ResponseModel> GetManageVenderDetails(long id)
        //{
        //    VenderDetailsResponse? dealer;

        //    if (id <= 0)
        //    {
        //        _response.IsSuccess = false;
        //        _response.Message = ValidationConstants.Id_Required_Msg;
        //    }
        //    else
        //    {
        //        dealer = await _vendorService.GetVenderDetailsById(id);
        //        _response.Data = dealer;
        //    }

        //    return _response;
        //}

        //#endregion
    }
}
