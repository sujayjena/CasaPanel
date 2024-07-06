using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace CasaAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageBoxSizeController : CustomBaseController
    {
        private ResponseModel _response;
        private IAdminService _adminService;
        private IFileManager _fileManager;
        public ManageBoxSizeController(IAdminService adminService, IFileManager fileManager)
        {
            _adminService = adminService;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetManageBoxSizeList(ManageBoxSizeSearchParameters request)
        {
            IEnumerable<ManageBoxSizeResponse> lstManageBoxSizes = await _adminService.GetManageBoxSizeList(request);
            _response.Data = lstManageBoxSizes.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetManageBoxSizeDetails(long id)
        {
            ManageBoxSizeResponse? manageBoxSizeResponse;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                manageBoxSizeResponse = await _adminService.GetManageBoxSizeById(id);
                _response.Data = manageBoxSizeResponse;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadManageBoxSizeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.ManageBoxSizeImportFormatFileName));
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
        public async Task<ResponseModel> SaveManageBoxSize(ManageBoxSizeModel Request)
        {
            int result = await _adminService.SaveManageBoxSize(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Record already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Manage Box Size saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportManageBoxListToExcel(ManageBoxSizeSearchParameters request)
        {
            IEnumerable<ManageBoxSizeResponse> manageBoxSizeResponse;

            request.IsExport = true;
            manageBoxSizeResponse = await _adminService.GetManageBoxSizeList(request);
            if (manageBoxSizeResponse != null && manageBoxSizeResponse.ToList().Count > 0)
            {
                _response.Data = GenerateExcelManageBoxSizeDataFile(manageBoxSizeResponse);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }

        private byte[] GenerateExcelManageBoxSizeDataFile(IEnumerable<ManageBoxSizeResponse> lstManageBoxSizeToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"ManageBoxSize_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("ManageBoxSize");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "TileSize Name";
                    excelWorksheet.Cells[1, 2].Value = "No Of Tiles Per Box";
                    excelWorksheet.Cells[1, 3].Value = "Weight Per Box";
                    excelWorksheet.Cells[1, 4].Value = "Thickness";
                    excelWorksheet.Cells[1, 5].Value = "Box Coverage Area SqFoot";
                    excelWorksheet.Cells[1, 6].Value = "Box Coverage Area SqMeter";
                    excelWorksheet.Cells[1, 7].Value = "IsActive";

                    recordIndex = 2;

                    foreach (ManageBoxSizeResponse record in lstManageBoxSizeToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.TileSizeName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.NoOfTilesPerBox;
                        excelWorksheet.Cells[recordIndex, 3].Value = record.WeightPerBox;
                        excelWorksheet.Cells[recordIndex, 4].Value = record.Thickness;
                        excelWorksheet.Cells[recordIndex, 5].Value = record.BoxCoverageAreaSqFoot;
                        excelWorksheet.Cells[recordIndex, 6].Value = record.BoxCoverageAreaSqMeter;
                        excelWorksheet.Cells[recordIndex, 7].Value = record.IsActive;
                        recordIndex += 1;
                    }

                    excelWorksheet.Column(1).AutoFit();
                    excelWorksheet.Column(2).AutoFit();
                    excelWorksheet.Column(3).AutoFit();
                    excelWorksheet.Column(4).AutoFit();
                    excelWorksheet.Column(5).AutoFit();
                    excelWorksheet.Column(6).AutoFit();
                    excelWorksheet.Column(7).AutoFit();

                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportManageBoxSize([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportManageBoxSize> lstImportManageBoxSize = new List<ImportManageBoxSize>();
            IEnumerable<ManageBoxSizeValidationErrors> lstManageBoxSizeFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Manage Box Size Data";
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

                if (
                    !string.Equals(workSheet.Cells[1, 1].Value.ToString(), "TileSizeName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "NoOfTilesPerBox", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "WeightPerBox", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "Thickness", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "BoxCoverageAreaSqFoot", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 6].Value.ToString(), "BoxCoverageAreaSqMeter", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 7].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {

                    lstImportManageBoxSize.Add(new ImportManageBoxSize()
                    {
                        TileSizeName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        NoOfTilesPerBox = Convert.ToInt32(workSheet.Cells[rowIterator, 2].Value?.ToString()),
                        WeightPerBox = Convert.ToInt32(workSheet.Cells[rowIterator, 3].Value),
                        Thickness = Convert.ToDecimal(workSheet.Cells[rowIterator, 4].Value),
                        BoxCoverageAreaSqFoot = Convert.ToDecimal(workSheet.Cells[rowIterator, 5].Value?.ToString()),
                        BoxCoverageAreaSqMeter = Convert.ToDecimal(workSheet.Cells[rowIterator, 6].Value?.ToString()),
                        IsActive = workSheet.Cells[rowIterator, 7].Value?.ToString()
                        
                    });
                }
            }

            if (lstImportManageBoxSize.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstManageBoxSizeFailedToImport = await _adminService.ImportManageBoxSize(lstImportManageBoxSize);

            _response.IsSuccess = true;
            _response.Message = "Manage Box Size list imported successfully.";

            #region Generate Excel file for Invalid Data
            if (lstImportManageBoxSize.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidProductDesignDataFile(lstManageBoxSizeFailedToImport);

            }
            #endregion

            return _response;
        }

        private byte[] GenerateInvalidProductDesignDataFile(IEnumerable<ManageBoxSizeValidationErrors> lstManageBoxSizeFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Manage_Box_Size_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "TileSizeName";
                    WorkSheet1.Cells[1, 2].Value = "NoOfTilesPerBox";
                    WorkSheet1.Cells[1, 3].Value = "WeightPerBox";
                    WorkSheet1.Cells[1, 4].Value = "Thickness";
                    WorkSheet1.Cells[1, 5].Value = "BoxCoverageAreaSqFoot";
                    WorkSheet1.Cells[1, 6].Value = "BoxCoverageAreaSqMeter";
                    WorkSheet1.Cells[1, 7].Value = "IsActive";
                    WorkSheet1.Cells[1, 8].Value = "ValidationMessage";


                    recordIndex = 2;

                    foreach (ManageBoxSizeValidationErrors record in lstManageBoxSizeFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.TileSizeName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.NoOfTilesPerBox;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.WeightPerBox;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.Thickness;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.BoxCoverageAreaSqFoot;
                        WorkSheet1.Cells[recordIndex, 6].Value = record.BoxCoverageAreaSqMeter;
                        WorkSheet1.Cells[recordIndex, 7].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 8].Value = record.ValidationMessage;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();
                    WorkSheet1.Column(5).AutoFit();
                    WorkSheet1.Column(6).AutoFit();
                    WorkSheet1.Column(7).AutoFit();
                    WorkSheet1.Column(8).AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }
    }
}
