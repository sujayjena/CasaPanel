using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using CasaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static CasaAPI.Models.DriverDeatilsModel;
using static CasaAPI.Models.VehicleModel;


namespace CasaAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : CustomBaseController
    {
        private ResponseModel _response;
        private IDriverService _driverService;
        private IFileManager _fileManager;
        public DriverController(IDriverService driverService, IFileManager fileManager)
        {
            _driverService = driverService;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region Vehicle Number
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveVehicle(VehicleSaveParameters Request)
        {
            int result = await _driverService.SaveVehicle(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Vehicle is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Vehicle details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetVehicleList(VehicleSearchParameters request)
        {
            IEnumerable<VehicleDetailsResponse> lstVehicles = await _driverService.GetVehiclesList(request);
            _response.Data = lstVehicles.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetVehicleDetails(long id)
        {
            VehicleDetailsResponse? Vehicle;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                Vehicle = await _driverService.GetVehicleDetailsById(id);
                _response.Data = Vehicle;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportVehiclesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<VehicleImportSaveParameters> lstVehicleImportDetails = new List<VehicleImportSaveParameters>();
            List<VehicleFailToImportValidationErrors>? lstVehiclesFailedToImport = new List<VehicleFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            VehicleImportSaveParameters tempVehicleImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Vehicle data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "VehicleNumber", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempVehicleImport = new VehicleImportSaveParameters()
                    {
                        VehicleNumber = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempVehicleImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstVehicleImportDetails.Add(tempVehicleImport);
                    }
                    else
                    {
                        lstVehiclesFailedToImport.Add(new VehicleFailToImportValidationErrors()
                        {
                            VehicleNumber = tempVehicleImport.VehicleNumber,
                            IsActive = tempVehicleImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstVehicleImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstVehiclesFailedToImport.AddRange(await _driverService.ImportVehiclesDetails(lstVehicleImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Vehicles list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstVehiclesFailedToImport != null && lstVehiclesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidVehicleDataFile(lstVehiclesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidVehicleDataFile(IEnumerable<VehicleFailToImportValidationErrors> lstVehiclesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Vehicle_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "VehicleGroup";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (VehicleFailToImportValidationErrors record in lstVehiclesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.VehicleNumber;
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
        public async Task<ResponseModel> DownloadVehicleTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.VehicleImportFormatFileName));
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
        public async Task<ResponseModel> ExportVehicleListToExcel(VehicleSearchParameters request)
        {
            IEnumerable<VehicleDetailsResponse> VehicleDetailsResponses;

            request.IsExport = true;
            VehicleDetailsResponses = await _driverService.GetVehiclesList(request);
            if (VehicleDetailsResponses != null && VehicleDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelVehicleDataFile(VehicleDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelVehicleDataFile(IEnumerable<VehicleDetailsResponse> lstVehiclesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Vehicle_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Vehicle");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Vehicle Group";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (VehicleDetailsResponse record in lstVehiclesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.VehicleNumber;
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

        #region Driver Details
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDriver(DriverSaveParameters request)
        {
            //Image Upload
            if (!string.IsNullOrWhiteSpace(request.ProfileSavedFileName_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(request.ProfileSavedFileName_Base64, "\\Uploads\\Driver\\", request.ProfileFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    request.ProfileSavedFileName = vUploadFile;
                }
            }

            int result = await _driverService.SaveDriver(request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Driver is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Driver details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDriverList(DriverSearchParameters request)
        {
            IEnumerable<DriverDetailsResponse> lstDrivers = await _driverService.GetDriversList(request);
            _response.Data = lstDrivers.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDriverDetails(long id)
        {
            DriverDetailsResponse? Driver;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                Driver = await _driverService.GetDriverDetailsById(id);
                _response.Data = Driver;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportDriversData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<DriverImportSaveParameters> lstDriverImportDetails = new List<DriverImportSaveParameters>();
            List<DriverFailToImportValidationErrors>? lstDriversFailedToImport = new List<DriverFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            DriverImportSaveParameters tempDriverImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Driver data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "DriverName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "VehicleNumber", StringComparison.OrdinalIgnoreCase) ||
                    //!string.Equals(workSheet.Cells[1, 3].Value.ToString(), "ProfilePath", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "MobileNumber", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempDriverImport = new DriverImportSaveParameters()
                    {
                        DriverName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        VehicleNumber = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                        //ProfilePath = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                        MobileNumber = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 4].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempDriverImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstDriverImportDetails.Add(tempDriverImport);
                    }
                    else
                    {
                        lstDriversFailedToImport.Add(new DriverFailToImportValidationErrors()
                        {
                            DriverName = tempDriverImport.DriverName,
                            VehicleNumber = tempDriverImport.VehicleNumber,
                            //ProfilePath = tempDriverImport.ProfilePath,
                            MobileNumber = tempDriverImport.MobileNumber,
                            IsActive = tempDriverImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstDriverImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstDriversFailedToImport.AddRange(await _driverService.ImportDriversDetails(lstDriverImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Drivers list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstDriversFailedToImport != null && lstDriversFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidDriverDataFile(lstDriversFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidDriverDataFile(IEnumerable<DriverFailToImportValidationErrors> lstDriversFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Driver_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "DriverName";
                    WorkSheet1.Cells[1, 2].Value = "MobileNumber";
                    WorkSheet1.Cells[1, 3].Value = "VehicleNumber";
                    //WorkSheet1.Cells[1, 4].Value = "ProfilePath";
                    WorkSheet1.Cells[1, 4].Value = "IsActive";
                    WorkSheet1.Cells[1, 5].Value = "ValidationMessage";

                    recordIndex = 6;

                    foreach (DriverFailToImportValidationErrors record in lstDriversFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.DriverName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.MobileNumber;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.VehicleNumber;
                        //WorkSheet1.Cells[recordIndex, 4].Value = record.ProfilePath;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();
                    WorkSheet1.Column(5).AutoFit();
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
        public async Task<ResponseModel> DownloadDriverTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.DriverImportFormatFileName));
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
        public async Task<ResponseModel> ExportDriverListToExcel(DriverSearchParameters request)
        {
            IEnumerable<DriverDetailsResponse> DriverDetailsResponses;

            request.IsExport = true;
            DriverDetailsResponses = await _driverService.GetDriversList(request);
            if (DriverDetailsResponses != null && DriverDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelDriverDataFile(DriverDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelDriverDataFile(IEnumerable<DriverDetailsResponse> lstDriversToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Driver_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Driver");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Driver Group";
                    excelWorksheet.Cells[1, 2].Value = "Mobile Number";
                    excelWorksheet.Cells[1, 3].Value = "Vehicle Number";
                    //excelWorksheet.Cells[1, 4].Value = "Profile Path";
                    excelWorksheet.Cells[1, 4].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (DriverDetailsResponse record in lstDriversToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.DriverName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.MobileNumber;
                        excelWorksheet.Cells[recordIndex, 3].Value = record.VehicleNumber;
                        //excelWorksheet.Cells[recordIndex, 4].Value = record.ProfilePath;
                        excelWorksheet.Cells[recordIndex, 4].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                    excelWorksheet.Column(3).AutoFit();
                    excelWorksheet.Column(4).AutoFit();
                    //excelWorksheet.Column(5).AutoFit();


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
