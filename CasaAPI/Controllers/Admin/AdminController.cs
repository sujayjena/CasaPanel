using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using CasaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

namespace CasaAPI.Controllers.Admin
{
    public class AdminController : CustomBaseController
    {
        private ResponseModel _response;
        private IAdminService _adminService;
        private IFileManager _fileManager;
        private IHubContext<MessageHubClient, IMessageHubClient> _messageHub;

        public AdminController(IAdminService adminService, IFileManager fileManager, IHubContext<MessageHubClient, IMessageHubClient> messageHub)
        {
            _adminService = adminService;
            _fileManager = fileManager;
            _messageHub = messageHub;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task GetNotification()
        {
            List<string> list = new List<string>() { "Notification 1", "Notification 2", "Notification 3" };
            await _messageHub.Clients.All.SendVisitNotificationToEmployee(list);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetUsersList()
        {
            IEnumerable<Users> lstUsers = await _adminService.GetUsersList();
            _response.Data = lstUsers.ToList();
            return _response;
        }

        #region Product API
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetProductsList(SearchProductRequest request)
        {
            IEnumerable<ProductResponse> lstProducts = await _adminService.GetProductsList(request);

            _response.Data = lstProducts;
            _response.Total = request.pagination.Total;
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportProductsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedProductDetails> lstImportedProductDetails = new List<ImportedProductDetails>();
            IEnumerable<ProductDataValidationErrors> lstProductsFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Product data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "ProductName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedProductDetails.Add(new ImportedProductDetails()
                    {
                        ProductName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedProductDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstProductsFailedToImport = await _adminService.ImportProductsDetails(lstImportedProductDetails);

            _response.IsSuccess = true;
            _response.Message = "Products list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstProductsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidProductDataFile(lstProductsFailedToImport);

            }
            #endregion

            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveProduct(ProductRequest productRequest)
        {
            int result = await _adminService.SaveProduct(productRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Product Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Product details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetProductDetails(long id)
        {
            ProductResponse? product;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                product = await _adminService.GetProductDetailsById(id);
                _response.Data = product;
            }

            return _response;
        }
        private byte[] GenerateInvalidProductDataFile(IEnumerable<ProductDataValidationErrors> lstProductsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Product_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "ProductName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (ProductDataValidationErrors record in lstProductsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.ProductName;
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

        #endregion Product API

        #region Design Type API

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDesignTypesList(SearchDesignTypeRequest request)
        {
            IEnumerable<DesignTypeResponse> lstDesignTypes = await _adminService.GetDesignTypesList(request);
            _response.Data = lstDesignTypes.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDesignType(DesignTypeRequest designTypeRequest)
        {
            int result = await _adminService.SaveDesignType(designTypeRequest);
            _response.IsSuccess = false;
            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Design Type Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Design Type details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDesignTypeDetails(long id)
        {
            DesignTypeResponse? product;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                product = await _adminService.GetDesignTypeDetailsById(id);
                _response.Data = product;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportDesignTypesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedDesignTypeDetails> lstImportedDesignTypeDetails = new List<ImportedDesignTypeDetails>();
            IEnumerable<DesignTypeDataValidationErrors> lstDesignTypesFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Design Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "DesignTypeName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedDesignTypeDetails.Add(new ImportedDesignTypeDetails()
                    {
                        DesignTypeName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedDesignTypeDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstDesignTypesFailedToImport = await _adminService.ImportDesignTypesDetails(lstImportedDesignTypeDetails);

            _response.IsSuccess = true;
            _response.Message = "Design Types list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstDesignTypesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidDesignTypeDataFile(lstDesignTypesFailedToImport);

            }
            #endregion

            return _response;
        }


        private byte[] GenerateInvalidDesignTypeDataFile(IEnumerable<DesignTypeDataValidationErrors> lstDesignTypesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_DesignType_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "DesignTypeName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (DesignTypeDataValidationErrors record in lstDesignTypesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.DesignTypeName;
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

        #endregion Design Type API

        #region Series API

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSeriesList(SearchSeriesRequest request)
        {
            IEnumerable<SeriesResponse> lstSeries = await _adminService.GetSeriesList(request);
            _response.Data = lstSeries.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveSeries(SeriesRequest seriesRequest)
        {
            int result = await _adminService.SaveSeries(seriesRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Series Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Series details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetSeriesDetails(long id)
        {
            SeriesResponse? series;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                series = await _adminService.GetSeriesDetailsById(id);
                _response.Data = series;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportSeriesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedSeriesDetails> lstImportedSeriesDetails = new List<ImportedSeriesDetails>();
            IEnumerable<SeriesDataValidationErrors> lstSeriesFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Series data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "SeriesName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedSeriesDetails.Add(new ImportedSeriesDetails()
                    {
                        SeriesName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedSeriesDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstSeriesFailedToImport = await _adminService.ImportSeriesDetails(lstImportedSeriesDetails);

            _response.IsSuccess = true;
            _response.Message = "Series list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstSeriesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidSeriesDataFile(lstSeriesFailedToImport);

            }
            #endregion

            return _response;
        }


        private byte[] GenerateInvalidSeriesDataFile(IEnumerable<SeriesDataValidationErrors> lstSeriesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Series_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "SeriesName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (SeriesDataValidationErrors record in lstSeriesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.SeriesName;
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
        [HttpPost]
        public async Task<ResponseModel> ExportSeriesData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new SearchSeriesRequest();
            request.pagination = new PaginationParameters();

            IEnumerable<SeriesResponse> lstSeriesObj = await _adminService.GetSeriesList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Series");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Series";
                    WorkSheet1.Cells[1, 2].Value = "Status";

                    WorkSheet1.Cells[1, 3].Value = "CreatedBy";
                    WorkSheet1.Cells[1, 4].Value = "CreatedDate";

                    recordIndex = 2;

                    foreach (var items in lstSeriesObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.SeriesName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 3].Value = items.CreatorName;
                        WorkSheet1.Cells[recordIndex, 4].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatedOn;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Series list Exported successfully";
            }

            return _response;
        }

        #endregion Series API

        #region Base Design API

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBaseDesignsList(SearchBaseDesignRequest request)
        {
            IEnumerable<BaseDesignResponse> lstbaseDesigns = await _adminService.GetBaseDesignsList(request);
            _response.Data = lstbaseDesigns.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]

        public async Task<ResponseModel> SaveBaseDesign(BaseDesignRequest baseDesignRequest)
        {
            int result = await _adminService.SaveBaseDesign(baseDesignRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Base Design Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Base Design details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetBaseDesignDetails(long id)
        {
            BaseDesignResponse? baseDesign;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                baseDesign = await _adminService.GetBaseDesignDetailsById(id);
                _response.Data = baseDesign;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]

        public async Task<ResponseModel> ImportBaseDesignsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedBaseDesignDetails> lstImportedBaseDesignDetails = new List<ImportedBaseDesignDetails>();
            IEnumerable<BaseDesignDataValidationErrors> lstBaseDesignsFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Base Design data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "BaseDesignName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedBaseDesignDetails.Add(new ImportedBaseDesignDetails()
                    {
                        BaseDesignName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedBaseDesignDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstBaseDesignsFailedToImport = await _adminService.ImportBaseDesignsDetails(lstImportedBaseDesignDetails);

            _response.IsSuccess = true;
            _response.Message = "Design Types list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstBaseDesignsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidBaseDesignDataFile(lstBaseDesignsFailedToImport);

            }
            #endregion

            return _response;
        }


        private byte[] GenerateInvalidBaseDesignDataFile(IEnumerable<BaseDesignDataValidationErrors> lstBaseDesignsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_BaseDesign_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "BaseDesignName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (BaseDesignDataValidationErrors record in lstBaseDesignsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.BaseDesignName;
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
        [HttpPost]
        public async Task<ResponseModel> ExportBaseDesignData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new SearchBaseDesignRequest();
            request.pagination = new PaginationParameters();

            IEnumerable<BaseDesignResponse> lstBaseDesignObj = await _adminService.GetBaseDesignsList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("BaseDesign");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "DesignName";
                    WorkSheet1.Cells[1, 2].Value = "Status";

                    WorkSheet1.Cells[1, 3].Value = "CreatedBy";
                    WorkSheet1.Cells[1, 4].Value = "CreatedDate";

                    recordIndex = 2;

                    foreach (var items in lstBaseDesignObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.BaseDesignName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 3].Value = items.CreatorName;
                        WorkSheet1.Cells[recordIndex, 4].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatedOn;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Base Design list Exported successfully";
            }

            return _response;
        }

        #endregion Base Design API

        #region Customer Type API

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCustomerTypesList(SearchCustomerTypeRequest request)
        {
            IEnumerable<CustomerTypeResponse> lstcustomTypes = await _adminService.GetCustomerTypesList(request);
            _response.Data = lstcustomTypes.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCustomerType(CustomerTypeRequest customerTypeRequest)
        {
            int result = await _adminService.SaveCustomerType(customerTypeRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Customer Type Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Customer Type details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCustomerTypeDetails(long id)
        {
            CustomerTypeResponse? custType;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                custType = await _adminService.GetCustomerTypeDetailsById(id);
                _response.Data = custType;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportCustomerTypesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedCustomerTypeDetails> lstImportedCustomerTypeDetails = new List<ImportedCustomerTypeDetails>();
            IEnumerable<CustomerTypeDataValidationErrors> lstCustomerTypesFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Customer Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "CustomerTypeName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedCustomerTypeDetails.Add(new ImportedCustomerTypeDetails()
                    {
                        CustomerTypeName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedCustomerTypeDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstCustomerTypesFailedToImport = await _adminService.ImportCustomerTypesDetails(lstImportedCustomerTypeDetails);

            _response.IsSuccess = true;
            _response.Message = "Customer Types list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstCustomerTypesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidCustomerTypeDataFile(lstCustomerTypesFailedToImport);

            }
            #endregion

            return _response;
        }


        private byte[] GenerateInvalidCustomerTypeDataFile(IEnumerable<CustomerTypeDataValidationErrors> lstCustomerTypesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_CustomerType_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CustomerTypeName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (CustomerTypeDataValidationErrors record in lstCustomerTypesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.CustomerTypeName;
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
        [HttpPost]
        public async Task<ResponseModel> ExportCustomerTypeData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new SearchCustomerTypeRequest();
            request.pagination = new PaginationParameters();

            IEnumerable<CustomerTypeResponse> lstCustomerTypeObj = await _adminService.GetCustomerTypesList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("CustomerType");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CustomerType";
                    WorkSheet1.Cells[1, 2].Value = "Status";

                    WorkSheet1.Cells[1, 3].Value = "CreatedBy";
                    WorkSheet1.Cells[1, 4].Value = "CreatedDate";

                    recordIndex = 2;

                    foreach (var items in lstCustomerTypeObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.CustomerTypeName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 3].Value = items.CreatorName;
                        WorkSheet1.Cells[recordIndex, 4].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatedOn;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Customer Type list Exported successfully";
            }

            return _response;
        }

        #endregion Customer Type API

        #region Leave Type API

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetLeaveTypesList(SearchLeaveTypeRequest request)
        {
            IEnumerable<LeaveTypeResponse> lstleaveTypes = await _adminService.GetLeaveTypesList(request);
            _response.Data = lstleaveTypes.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveLeaveType(LeaveTypeRequest leaveTypeRequest)
        {
            int result = await _adminService.SaveLeaveType(leaveTypeRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Leave Type Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Leave Type details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetLeaveTypeDetails(long id)
        {
            LeaveTypeResponse? leaveType;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                leaveType = await _adminService.GetLeaveTypeDetailsById(id);
                _response.Data = leaveType;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportLeaveTypesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedLeaveTypeDetails> lstImportedLeaveTypeDetails = new List<ImportedLeaveTypeDetails>();
            IEnumerable<LeaveTypeDataValidationErrors> lstLeaveTypesFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Leave Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "LeaveTypeName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedLeaveTypeDetails.Add(new ImportedLeaveTypeDetails()
                    {
                        LeaveTypeName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedLeaveTypeDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstLeaveTypesFailedToImport = await _adminService.ImportLeaveTypesDetails(lstImportedLeaveTypeDetails);

            _response.IsSuccess = true;
            _response.Message = "Leave Types list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstLeaveTypesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidLeaveTypeDataFile(lstLeaveTypesFailedToImport);

            }
            #endregion

            return _response;
        }


        private byte[] GenerateInvalidLeaveTypeDataFile(IEnumerable<LeaveTypeDataValidationErrors> lstLeaveTypesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_LeaveType_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "LeaveTypeName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (LeaveTypeDataValidationErrors record in lstLeaveTypesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.LeaveTypeName;
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
        [HttpPost]
        public async Task<ResponseModel> ExportLeaveTypeData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new SearchLeaveTypeRequest();
            request.pagination = new PaginationParameters();

            IEnumerable<LeaveTypeResponse> lstLeaveTypeObj = await _adminService.GetLeaveTypesList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("LeaveType");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "LeaveType";
                    WorkSheet1.Cells[1, 2].Value = "Status";

                    WorkSheet1.Cells[1, 3].Value = "CreatedBy";
                    WorkSheet1.Cells[1, 4].Value = "CreatedDate";

                    recordIndex = 2;

                    foreach (var items in lstLeaveTypeObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.LeaveTypeName;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 3].Value = items.CreatorName;
                        WorkSheet1.Cells[recordIndex, 4].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatedOn;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Leave Type list Exported successfully";
            }

            return _response;
        }

        #endregion Leave Type 

        #region Blood Group Master

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBloodGroupDetails(BloodGroupRequestModel parameters)
        {
            int result = await _adminService.SaveBloodGroupDetails(parameters);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Blood Group value is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Blood Group details saved sucessfully";
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBloodGroupList(SearchBloodGroupRequestModel parameters)
        {
            IEnumerable<BloodGroupResponseModel> lstBloodGroups = await _adminService.GetBloodGroupList(parameters);
            _response.Data = lstBloodGroups.ToList();
            _response.Total = parameters.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetBloodGroupDetails(int id)
        {
            BloodGroupResponseModel? bloodGroup;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                bloodGroup = await _adminService.GetBloodGroupDetails(id);
                _response.Data = bloodGroup;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ExportBloodGroupData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new SearchBloodGroupRequestModel();
            request.pagination = new PaginationParameters();

            IEnumerable<BloodGroupResponseModel> lstBloodGroupObj = await _adminService.GetBloodGroupList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("BloodGroup");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "BloodGroup";
                    WorkSheet1.Cells[1, 2].Value = "Status";

                    WorkSheet1.Cells[1, 3].Value = "CreatedBy";
                    WorkSheet1.Cells[1, 4].Value = "CreatedDate";

                    recordIndex = 2;

                    foreach (var items in lstBloodGroupObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.BloodGroup;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 3].Value = items.CreatorName;
                        WorkSheet1.Cells[recordIndex, 4].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatedOn;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Blood Group list Exported successfully";
            }

            return _response;
        }

        #endregion

        #region Collection Master

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCollectionDetails(SaveCollectionRequestModel parameters)
        {
            int result = await _adminService.SaveCollectionMasterDetails(parameters);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Collection is already exists";
            }
            else if (result == (int)SaveEnums.CodeExists)
            {
                _response.Message = "Collection Id is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Collection details saved sucessfully";
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCollectionMasterList(SearchCollectionRequestModel parameters)
        {
            IEnumerable<CollectionResponseModel> lstCollections = await _adminService.GetCollectionMasterList(parameters);
            _response.Data = lstCollections.ToList();
            _response.Total = parameters.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCollectionMasterDetails(int id)
        {
            CollectionResponseModel? collection;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                collection = await _adminService.GetCollectionMasterDetails(id);
                _response.Data = collection;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadTemplateToImportCollection()
        {
            byte[]? formatFile = await Task.Run(() => _fileManager.GetFormatFileFromPath("ImportCollectionFormat.xlsx"));

            if (formatFile != null)
            {
                _response.Data = formatFile;
            }
            else
            {

            }

            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportCollectionData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportedCollection> lstImportedCollection = new List<ImportedCollection>();
            IEnumerable<CollectionDataValidationErrors> lstCollectionsFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Collection data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "CollectionName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    lstImportedCollection.Add(new ImportedCollection()
                    {
                        CollectionName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    });
                }
            }

            if (lstImportedCollection.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstCollectionsFailedToImport = await _adminService.ImportCollection(lstImportedCollection);

            _response.IsSuccess = true;
            _response.Message = "Categorys list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstCollectionsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidCollectionDataFile(lstCollectionsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidCollectionDataFile(IEnumerable<CollectionDataValidationErrors> lstCollectionFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Collection_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CollectionName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (CollectionDataValidationErrors record in lstCollectionFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.CollectionName;
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
        [HttpPost]
        public async Task<ResponseModel> ExportCollectionData()
        {
            _response.IsSuccess = false;
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var request = new SearchCollectionRequestModel();
            request.pagination = new PaginationParameters();

            IEnumerable<CollectionResponseModel> lstCollectionObj = await _adminService.GetCollectionMasterList(request);

            using (MemoryStream msExportDataFile = new MemoryStream())
            {
                using (ExcelPackage excelExportData = new ExcelPackage())
                {
                    WorkSheet1 = excelExportData.Workbook.Worksheets.Add("Collection");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CollectionId";
                    WorkSheet1.Cells[1, 2].Value = "Collection";
                    WorkSheet1.Cells[1, 3].Value = "Status";

                    WorkSheet1.Cells[1, 4].Value = "CreatedBy";
                    WorkSheet1.Cells[1, 5].Value = "CreatedDate";

                    recordIndex = 2;

                    foreach (var items in lstCollectionObj)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = items.CollectionNameId;
                        WorkSheet1.Cells[recordIndex, 2].Value = items.CollectionName;
                        WorkSheet1.Cells[recordIndex, 3].Value = items.IsActive == true ? "Active" : "Inactive";

                        WorkSheet1.Cells[recordIndex, 4].Value = items.CreatorName;
                        WorkSheet1.Cells[recordIndex, 5].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                        WorkSheet1.Cells[recordIndex, 5].Value = items.CreatedOn;

                        recordIndex += 1;
                    }

                    WorkSheet1.Column(1).AutoFit();
                    WorkSheet1.Column(2).AutoFit();
                    WorkSheet1.Column(3).AutoFit();
                    WorkSheet1.Column(4).AutoFit();
                    WorkSheet1.Column(5).AutoFit();

                    excelExportData.SaveAs(msExportDataFile);
                    msExportDataFile.Position = 0;
                    result = msExportDataFile.ToArray();
                }
            }

            if (result != null)
            {
                _response.Data = result;
                _response.IsSuccess = true;
                _response.Message = "Collection list Exported successfully";
            }

            return _response;
        }
        #endregion
    }
}
