using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CasaAPI.Controllers
{
    public class ManageTerritoryController : CustomBaseController
    {
        private ResponseModel _response;
        private IManageTerritoryService _manageTerritorService;
        private IFileManager _fileManager;

        public ManageTerritoryController(IManageTerritoryService manageTerritorService, IFileManager fileManager)
        {
            _manageTerritorService = manageTerritorService;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region State API
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetStatesList(SearchStateRequest request)
        {
            IEnumerable<StateResponse> lstStates = await _manageTerritorService.GetStatesList(request);
            _response.Data = lstStates.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveState(StateRequest stateRequest)
        {
            int result = await _manageTerritorService.SaveState(stateRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "State Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "State details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetStateDetails(long id)
        {
            StateResponse? state;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                state = await _manageTerritorService.GetStateDetailsById(id);
                _response.Data = state;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetStateDetailsByRegionId(long regionId)
        {
                IEnumerable<SelectListResponse> lstResponse = await _manageTerritorService.GetStateDetailsByRegionId(regionId); 
                _response.Data = lstResponse.ToList();
                return _response;
            }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportStatesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedStateDetails> lstImportedStateDetails = new List<ImportedStateDetails>();
            IEnumerable<StateDataValidationErrors> lstStatesFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import State data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "StateName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedStateDetails.Add(new ImportedStateDetails()
                    {
                        StateName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedStateDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstStatesFailedToImport = await _manageTerritorService.ImportStatesDetails(lstImportedStateDetails);

            _response.IsSuccess = true;
            _response.Message = "States list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstStatesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidStateDataFile(lstStatesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidStateDataFile(IEnumerable<StateDataValidationErrors> lstStatesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_State_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "StateName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (StateDataValidationErrors record in lstStatesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.StateName;
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
        public async Task<ResponseModel> DownloadStateDataTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.StateImportFormatFileName));
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
        public async Task<ResponseModel> ExportStateListToExcel(SearchStateRequest request)
        {
            IEnumerable<StateResponse> stateResponse;

            request.IsExport = true;
            stateResponse = await _manageTerritorService.GetStatesList(request);
            if (stateResponse != null && stateResponse.ToList().Count > 0)
            {
                _response.Data = GenerateExcelStateDataFile(stateResponse);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }

        private byte[] GenerateExcelStateDataFile(IEnumerable<StateResponse> lstStatesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"State_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("State");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "State Name";
                    excelWorksheet.Cells[1, 2].Value = "IsActive";

                    recordIndex = 2;

                    foreach (StateResponse record in lstStatesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.StateName;
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

        #region Region API
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetRegionsList(SearchRegionRequest request)
        {
            IEnumerable<RegionResponse> lstRegions = await _manageTerritorService.GetRegionsList(request);
            _response.Data = lstRegions.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveRegion(RegionRequest regionRequest)
        {
            int result = await _manageTerritorService.SaveRegion(regionRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Region Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Region details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetRegionDetails(long id)
        {
            RegionResponse? region;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                region = await _manageTerritorService.GetRegionDetailsById(id);
                _response.Data = region;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportRegionsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedRegionDetails> lstImportedRegionDetails = new List<ImportedRegionDetails>();
            IEnumerable<RegionDataValidationErrors> lstRegionsFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Region data";
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

                if (!string.Equals(workSheet.Cells[1, 2].Value.ToString(), "RegionName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedRegionDetails.Add(new ImportedRegionDetails()
                    {
                        //StateName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        RegionName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedRegionDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstRegionsFailedToImport = await _manageTerritorService.ImportRegionsDetails(lstImportedRegionDetails);

            _response.IsSuccess = true;
            _response.Message = "Regions list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstRegionsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidRegionDataFile(lstRegionsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidRegionDataFile(IEnumerable<RegionDataValidationErrors> lstRegionsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Region_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    //WorkSheet1.Cells[1, 1].Value = "StateName";
                    WorkSheet1.Cells[1, 1].Value = "RegionName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (RegionDataValidationErrors record in lstRegionsFailedToImport)
                    {
                       // WorkSheet1.Cells[recordIndex, 1].Value = record.StateName;
                        WorkSheet1.Cells[recordIndex, 1].Value = record.RegionName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                   // WorkSheet1.Column(4).AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadRegionDataTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.RegionImportFormatFileName));
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
        public async Task<ResponseModel> ExportRegionListToExcel(SearchRegionRequest request)
        {
            IEnumerable<RegionResponse> regionResponse;

            request.IsExport = true;
            regionResponse = await _manageTerritorService.GetRegionsList(request);
            if (regionResponse != null && regionResponse.ToList().Count > 0)
            {
                _response.Data = GenerateExcelRegionDataFile(regionResponse);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }

        private byte[] GenerateExcelRegionDataFile(IEnumerable<RegionResponse> lstRegionsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Region_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Region");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Region Name";
                   // excelWorksheet.Cells[1, 2].Value = "State Name";
                    excelWorksheet.Cells[1, 2].Value = "IsActive";

                    recordIndex = 2;

                    foreach (RegionResponse record in lstRegionsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.RegionName;
                       // excelWorksheet.Cells[recordIndex, 2].Value = record.StateName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                   // excelWorksheet.Column(3).AutoFit();

                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }
        #endregion

        #region District API
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDistrictsList(SearchDistrictRequest request)
        {
            IEnumerable<DistrictResponse> lstDistricts = await _manageTerritorService.GetDistrictsList(request);
            _response.Data = lstDistricts.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDistrict(DistrictRequest districtRequest)
        {
            int result = await _manageTerritorService.SaveDistrict(districtRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "District Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "District details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDistrictDetails(long id)
        {
            DistrictResponse? district;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                district = await _manageTerritorService.GetDistrictDetailsById(id);
                _response.Data = district;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDistrictDetailsByStateId(long stateId)
        {
            IEnumerable<SelectListResponse> lstResponse = await _manageTerritorService.GetDistrictDetailsByStateId(stateId);
                _response.Data = lstResponse.ToList();
            
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportDistrictsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedDistrictDetails> lstImportedDistrictDetails = new List<ImportedDistrictDetails>();
            IEnumerable<DistrictDataValidationErrors> lstDistrictsFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import District data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "DistrictName", StringComparison.OrdinalIgnoreCase) ||                    
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedDistrictDetails.Add(new ImportedDistrictDetails()
                    {
                        DistrictName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        //RegionName = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                        //StateName = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedDistrictDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstDistrictsFailedToImport = await _manageTerritorService.ImportDistrictsDetails(lstImportedDistrictDetails);

            _response.IsSuccess = true;
            _response.Message = "Districts list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstDistrictsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidDistrictDataFile(lstDistrictsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidDistrictDataFile(IEnumerable<DistrictDataValidationErrors> lstDistrictsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_District_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "DistrictName";
                    //WorkSheet1.Cells[1, 2].Value = "RegionName";
                    //WorkSheet1.Cells[1, 3].Value = "StateName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (DistrictDataValidationErrors record in lstDistrictsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.DistrictName;
                        //WorkSheet1.Cells[recordIndex, 2].Value = record.RegionName;
                        //WorkSheet1.Cells[recordIndex, 3].Value = record.StateName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    //WorkSheet1.Column(4).AutoFit();
                    //WorkSheet1.Column(5).AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadDistrictDataTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.DistrictImportFormatFileName));
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
        public async Task<ResponseModel> ExportDistrictListToExcel(SearchDistrictRequest request)
        {
            IEnumerable<DistrictResponse> districtResponse;

            request.IsExport = true;
            districtResponse = await _manageTerritorService.GetDistrictsList(request);
            if (districtResponse != null && districtResponse.ToList().Count > 0)
            {
                _response.Data = GenerateExcelDistrictDataFile(districtResponse);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }

        private byte[] GenerateExcelDistrictDataFile(IEnumerable<DistrictResponse> lstDistrictResponse)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"District_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("District");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "District Name";
                    //excelWorksheet.Cells[1, 2].Value = "Region Name";
                    //excelWorksheet.Cells[1, 3].Value = "State Name";
                    excelWorksheet.Cells[1, 2].Value = "IsActive";

                    recordIndex = 2;

                    foreach (DistrictResponse record in lstDistrictResponse)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.DistrictName;
                        //excelWorksheet.Cells[recordIndex, 2].Value = record.RegionName;
                        //excelWorksheet.Cells[recordIndex, 3].Value = record.StateName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                    //excelWorksheet.Column(3).AutoFit();
                    //excelWorksheet.Column(4).AutoFit();

                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }
        #endregion

        #region City API
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCityList(SearchCityRequest request)
        {
            IEnumerable<CityResponse> lstCities = await _manageTerritorService.GetCityList(request);
            _response.Data = lstCities.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCity(CityRequest cityRequest)
        {
            int result = await _manageTerritorService.SaveCity(cityRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "City Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "City details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCityDetails(long id)
        {
            CityResponse? cityResponse;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                cityResponse = await _manageTerritorService.GetCityDetailsById(id);
                _response.Data = cityResponse;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCityDetailsByDistrictId(long DistrictId)
        {
            IEnumerable<SelectListResponse> lstResponse = await _manageTerritorService.GetCityDetailsByDistrictId(DistrictId);
            _response.Data = lstResponse.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportCityData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedCityDetails> lstImportedCityDetails = new List<ImportedCityDetails>();
            IEnumerable<CityDataValidationErrors> lstCitiesFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import City data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "CityName", StringComparison.OrdinalIgnoreCase) ||                    
                    !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedCityDetails.Add(new ImportedCityDetails()
                    {
                        CityName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        //DistrictName = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                        //RegionName = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                        //StateName = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedCityDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstCitiesFailedToImport = await _manageTerritorService.ImportCityDetails(lstImportedCityDetails);

            _response.IsSuccess = true;
            _response.Message = "City list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstCitiesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidCityDataFile(lstCitiesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidCityDataFile(IEnumerable<CityDataValidationErrors> lstCitiesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_City_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CityName";
                    //WorkSheet1.Cells[1, 2].Value = "DistrictName";
                    //WorkSheet1.Cells[1, 3].Value = "RegionName";
                    //WorkSheet1.Cells[1, 4].Value = "StateName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (CityDataValidationErrors record in lstCitiesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.CityName;
                        //WorkSheet1.Cells[recordIndex, 2].Value = record.DistrictName;
                        //WorkSheet1.Cells[recordIndex, 3].Value = record.RegionName;
                        //WorkSheet1.Cells[recordIndex, 4].Value = record.StateName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    //WorkSheet1.Column(4).AutoFit();
                    //WorkSheet1.Column(5).AutoFit();
                    //WorkSheet1.Column(6).AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadCityDataTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.CityImportFormatFileName));
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
        public async Task<ResponseModel> ExportCityListToExcel(SearchCityRequest request)
        {
            IEnumerable<CityResponse> cityResponse;

            request.IsExport = true;
            cityResponse = await _manageTerritorService.GetCityList(request);
            if (cityResponse != null && cityResponse.ToList().Count > 0)
            {
                _response.Data = GenerateExcelCityDataFile(cityResponse);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }

        private byte[] GenerateExcelCityDataFile(IEnumerable<CityResponse> lstCityResponse)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"City_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("City");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "City Name";
                    //excelWorksheet.Cells[1, 2].Value = "District Name";
                    //excelWorksheet.Cells[1, 3].Value = "Region Name";
                    //excelWorksheet.Cells[1, 4].Value = "State Name";
                    excelWorksheet.Cells[1, 2].Value = "IsActive";

                    recordIndex = 2;

                    foreach (CityResponse record in lstCityResponse)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.CityName;
                        //excelWorksheet.Cells[recordIndex, 2].Value = record.DistrictName;
                        //excelWorksheet.Cells[recordIndex, 3].Value = record.RegionName;
                        //excelWorksheet.Cells[recordIndex, 4].Value = record.StateName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                    //excelWorksheet.Column(3).AutoFit();
                    //excelWorksheet.Column(4).AutoFit();
                    //excelWorksheet.Column(5).AutoFit();

                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }
        #endregion

        #region Area API
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetAreasList(SearchAreaRequest request)
        {
            IEnumerable<AreaResponse> lstAreas = await _manageTerritorService.GetAreasList(request);
            _response.Data = lstAreas.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveArea(AreaRequest areaRequest)
        {
            int result = await _manageTerritorService.SaveArea(areaRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Area Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Area details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetAreaDetails(long id)
        {
            AreaResponse? area;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                area = await _manageTerritorService.GetAreaDetailsById(id);
                _response.Data = area;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetAreaDetailsByCityId(long CityId)
        {
            IEnumerable<SelectListResponse> lstResponse = await _manageTerritorService.GetAreaDetailsByCityId(CityId);
            _response.Data = lstResponse.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportAreasData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedAreaDetails> lstImportedAreaDetails = new List<ImportedAreaDetails>();
            IEnumerable<AreaDataValidationErrors> lstAreasFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Area data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "AreaName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "CityName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "DistrictName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "RegionName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "StateName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedAreaDetails.Add(new ImportedAreaDetails()
                    {
                        AreaName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        CityName = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                        DistrictName = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                        RegionName = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                        StateName = workSheet.Cells[rowIterator, 5].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 6].Value?.ToString()
                    });
                }
            }

            if (lstImportedAreaDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstAreasFailedToImport = await _manageTerritorService.ImportAreasDetails(lstImportedAreaDetails);

            _response.IsSuccess = true;
            _response.Message = "Areas list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstAreasFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidAreaDataFile(lstAreasFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidAreaDataFile(IEnumerable<AreaDataValidationErrors> lstAreasFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Area_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "AreaName";
                    //WorkSheet1.Cells[1, 2].Value = "CityName";
                    //WorkSheet1.Cells[1, 3].Value = "DistrictName";
                    //WorkSheet1.Cells[1, 4].Value = "RegionName";
                    //WorkSheet1.Cells[1, 5].Value = "StateName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (AreaDataValidationErrors record in lstAreasFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.AreaName;
                        //WorkSheet1.Cells[recordIndex, 2].Value = record.CityName;
                        //WorkSheet1.Cells[recordIndex, 3].Value = record.DistrictName;
                        //WorkSheet1.Cells[recordIndex, 4].Value = record.RegionName;
                        //WorkSheet1.Cells[recordIndex, 5].Value = record.StateName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    //WorkSheet1.Column(4).AutoFit();
                    //WorkSheet1.Column(5).AutoFit();
                    //WorkSheet1.Column(6).AutoFit();
                    //WorkSheet1.Column(7).AutoFit();


                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadAreaDataTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.AreaImportFormatFileName));
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
        public async Task<ResponseModel> ExportAreaListToExcel(SearchAreaRequest request)
        {
            IEnumerable<AreaResponse> areaResponse;

            request.IsExport = true;
            areaResponse = await _manageTerritorService.GetAreasList(request);
            if (areaResponse != null && areaResponse.ToList().Count > 0)
            {
                _response.Data = GenerateExcelAreaDataFile(areaResponse);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }

        private byte[] GenerateExcelAreaDataFile(IEnumerable<AreaResponse> lstAreaResponse)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Area_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Area");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Area Name";
                    //excelWorksheet.Cells[1, 2].Value = "City Name";
                    //excelWorksheet.Cells[1, 3].Value = "District Name";
                    //excelWorksheet.Cells[1, 4].Value = "Region Name";
                    //excelWorksheet.Cells[1, 5].Value = "State Name";
                    excelWorksheet.Cells[1, 2].Value = "IsActive";

                    recordIndex = 2;

                    foreach (AreaResponse record in lstAreaResponse)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.AreaName;
                        //excelWorksheet.Cells[recordIndex, 2].Value = record.CityName;
                        //excelWorksheet.Cells[recordIndex, 3].Value = record.DistrictName;
                        //excelWorksheet.Cells[recordIndex, 4].Value = record.RegionName;
                        //excelWorksheet.Cells[recordIndex, 5].Value = record.StateName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                    //excelWorksheet.Column(3).AutoFit();
                    //excelWorksheet.Column(4).AutoFit();
                    //excelWorksheet.Column(5).AutoFit();
                    //excelWorksheet.Column(6).AutoFit();

                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }
        #endregion

        #region Mapping
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveAreaTerritory(SaveAreamapping mappingRequest)
        {
            int result = await _manageTerritorService.SaveareaTerritory(mappingRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Area mapping Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Area Mapping details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetAreaTerritoryList(SearchAreaMappingRequest request)
        {
            IEnumerable<AreaMappingResponse> lstArea = await _manageTerritorService.GetAreaTerritoryList(request);
            _response.Data = lstArea.ToList();
            return _response;
        }
       
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetAreaMappingDetails(long id)
        {
            AreaMappingResponse? area;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                area = await _manageTerritorService.GetAreaMappingDetailsById(id);
                _response.Data = area;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTerritories_State_Dist_City_Area_List_ById(Territories_State_Dist_City_Area_Search parameters)
        {
            var vResultObj = await _manageTerritorService.GetTerritories_State_Dist_City_Area_List_ById(parameters);
            _response.Data = vResultObj;

            return _response;
        }

        #endregion
    }
}
