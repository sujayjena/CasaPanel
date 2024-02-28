using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CasaAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageProductDesignController : CustomBaseController
    {
        private ResponseModel _response;
        private IAdminService _adminService;
        private IFileManager _fileManager;
        public ManageProductDesignController(IAdminService adminService, IFileManager fileManager)
        {
            _adminService = adminService;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetProductDesignList(ProductDesignSearchParameters request)
        {
            IEnumerable<ProductDesignResponse> lstProductDesigns = await _adminService.GetProductDesignList(request);
            //List<EmployeeDetailsResponse> datalist = new List<EmployeeDetailsResponse>();
            //if (lstProductDesigns != null && lstProductDesigns.ToList().Count > 0)
            //{
            //    foreach (ProductDesignResponse record in lstProductDesigns)
            //    {
            //        ProductDesignResponse data = new ProductDesignResponse();
            //        data.ProductDesignId = record.ProductDesignId;
            //        data.PunchName = record.PunchName;
            //        data.SizeName= record.SizeName;
            //        data.SurfaceName= record.SurfaceName;
            //        data.ThicknessName= record.ThicknessName;
            //        data.TileSizeName= record.TileSizeName;
            //        data.TotalRecords= record.TotalRecords;
            //        data.NoOfTilesPerBox= record.NoOfTilesPerBox;
            //        data.WeightPerBox= record.WeightPerBox;
            //        data.BoxCoverageAreaSqMeter= record.BoxCoverageAreaSqMeter;
            //        data.BoxCoverageAreaSqFoot= record.BoxCoverageAreaSqFoot;
            //        data.BrandName= record.BrandName;
            //        data.CollectionName= record.CollectionName;
            //        data.CategoryName= record.CategoryName;
            //        data.DesignName= record.DesignName;
            //        data.TypeName= record.TypeName;
            //        data.CreatorName = record.CreatorName;
            //        data.CreatedOn = record.CreatedOn;
            //        data.IsActive = record.IsActive;
            //        datalist.Add(data);
            //    }
            //}
                    _response.Data = lstProductDesigns.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetProductDesignDetails(long id)
        {
            ProductDesignDetailsResponse? productDesignDetailsResponse;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                productDesignDetailsResponse = await _adminService.GetProductDesignDetailsById(id);
                if(productDesignDetailsResponse != null)
                {
                    productDesignDetailsResponse.ProductDesignFilesList = (await _adminService.GetProductDesignFiles(id)).ToList();
                  
                }
                _response.Data = productDesignDetailsResponse;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveProductDesign([FromForm] ProductDesignsSaveModel parameter)
        {
            Regex regex;
            int result;
          //  List<string> invalidFileNames = new List<string>();
            _response.IsSuccess = false;

           
            List<ProductDesignFiles> listToAdd = new List<ProductDesignFiles>();

           
            if (parameter?.DesignFiles?.Count > 0)
            {
                parameter.DesignName = parameter.DesignName;
                int flag = 0;
                foreach (IFormFile file in parameter.DesignFiles)
                {
                    if (flag == 0)
                    {
                        flag=1;
                        parameter.ProductDesignFiles = new List<ProductDesignFiles>()
                    {
                                new ProductDesignFiles()
                                {
                                FileType=file.ContentType,
                        UploadedFilesName = file.FileName,
                        DesignFile = _fileManager.UploadProductDesignFile(file)
                                },
                    };
                    }
                    else
                    {
                    listToAdd = new List<ProductDesignFiles>()
                       {
                                new ProductDesignFiles()
                                {
                        FileType=file.ContentType,
                        UploadedFilesName = file.FileName,
                        DesignFile = _fileManager.UploadProductDesignFile(file)
                                },
                    };
                        parameter.ProductDesignFiles.AddRange(listToAdd);
                    }
                    //var pdfiles = new ProductDesignFiles
                    //{
                    //    DesignFile="test",
                    //    FileType="tt",
                    //    UploadedFilesName = file.FileName,
                    //    StoredFilesName = _fileManager.UploadProfilePicture(file)
                    //};
                    //ProductDesignFiles data = new ProductDesignFiles();
                    //data.UploadedFilesName = file.FileName;
                    //data.StoredFilesName= _fileManager.UploadProfilePicture(file);
                    //listToAdd.Add(data);
                    //parameter.ProductDesignFiles.AddRange(listToAdd);
                }
            }
            
            #region Product Design form Validation check
            _response = ModelStateHelper.GetValidationErrorsList(parameter);

            if (!_response.IsSuccess)
            {
                return _response;
            }
            #endregion

            result = await _adminService.SaveProductDesign(parameter);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Design Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Product Design saved sucessfully";
            }

            return _response;
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportProductDesign([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ImportProductDesign> lstImportProductDesign = new List<ImportProductDesign>();
            IEnumerable<ProductDesignValidationErrors> lstProductDesignFailedToImport;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Product Design data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "DesignName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "SizeName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "BrandName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "CollectionName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "CategoryName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 6].Value.ToString(), "TypeName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 7].Value.ToString(), "PunchName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 8].Value.ToString(), "SurfaceName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 9].Value.ToString(), "ThicknessName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 10].Value.ToString(), "TileSizeName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 11].Value.ToString(), "NoOfTilesPerBox", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 12].Value.ToString(), "WeightPerBox", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 13].Value.ToString(), "BoxCoverageAreaSqFoot", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 14].Value.ToString(), "BoxCoverageAreaSqMeter", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 15].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {

                    lstImportProductDesign.Add(new ImportProductDesign()
                    {
                        DesignName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        SizeName = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                        BrandName = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                        CollectionName = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                        CategoryName = workSheet.Cells[rowIterator, 5].Value?.ToString(),
                        TypeName = workSheet.Cells[rowIterator, 6].Value?.ToString(),
                        PunchName = workSheet.Cells[rowIterator, 7].Value?.ToString(),
                        SurfaceName = workSheet.Cells[rowIterator, 8].Value?.ToString(),
                        ThicknessName = workSheet.Cells[rowIterator, 9].Value?.ToString(),
                        TileSizeName = workSheet.Cells[rowIterator, 10].Value?.ToString(),
                        NoOfTilesPerBox = Convert.ToInt32(workSheet.Cells[rowIterator, 11].Value?.ToString()),
                        WeightPerBox = Convert.ToInt32(workSheet.Cells[rowIterator, 12].Value),
                        BoxCoverageAreaSqFoot = Convert.ToDecimal(workSheet.Cells[rowIterator, 13].Value?.ToString()),
                        BoxCoverageAreaSqMeter = Convert.ToDecimal(workSheet.Cells[rowIterator, 14].Value?.ToString()),
                        IsActive = workSheet.Cells[rowIterator, 15].Value?.ToString()
                    });
                }
            }

            if (lstImportProductDesign.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstProductDesignFailedToImport = await _adminService.ImportProductDesignData(lstImportProductDesign);

            _response.IsSuccess = true;
            _response.Message = "Product design list imported successfully.";

            #region Generate Excel file for Invalid Data
            if (lstImportProductDesign.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidProductDesignDataFile(lstProductDesignFailedToImport);

            }
            #endregion

            return _response;
        }

        private byte[] GenerateInvalidProductDesignDataFile(IEnumerable<ProductDesignValidationErrors> lstProductDesignFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Product_Design_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "DesignName";
                    WorkSheet1.Cells[1, 2].Value = "SizeName";
                    WorkSheet1.Cells[1, 3].Value = "BrandName";
                    WorkSheet1.Cells[1, 4].Value = "CollectionName";
                    WorkSheet1.Cells[1, 5].Value = "CategoryName";
                    WorkSheet1.Cells[1, 6].Value = "TypeName";
                    WorkSheet1.Cells[1, 7].Value = "PunchName";
                    WorkSheet1.Cells[1, 8].Value = "SurfaceName";
                    WorkSheet1.Cells[1, 9].Value = "ThicknessName";
                    WorkSheet1.Cells[1, 10].Value = "TileSizeName";
                    WorkSheet1.Cells[1, 11].Value = "NoOfTilesPerBox";
                    WorkSheet1.Cells[1, 12].Value = "WeightPerBox";
                    WorkSheet1.Cells[1, 13].Value = "BoxCoverageAreaSqFoot";
                    WorkSheet1.Cells[1, 14].Value = "BoxCoverageAreaSqMeter";
                    WorkSheet1.Cells[1, 15].Value = "IsActive";
                    WorkSheet1.Cells[1, 16].Value = "ValidationMessage";


                    recordIndex = 2;

                    foreach (ProductDesignValidationErrors record in lstProductDesignFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.DesignName;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.SizeName;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.BrandName;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.CollectionName;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.CategoryName;
                        WorkSheet1.Cells[recordIndex, 6].Value = record.TypeName;
                        WorkSheet1.Cells[recordIndex, 7].Value = record.PunchName;
                        WorkSheet1.Cells[recordIndex, 8].Value = record.SurfaceName;
                        WorkSheet1.Cells[recordIndex, 9].Value = record.ThicknessName;
                        WorkSheet1.Cells[recordIndex, 10].Value = record.TileSizeName;
                        WorkSheet1.Cells[recordIndex, 11].Value = record.NoOfTilesPerBox;
                        WorkSheet1.Cells[recordIndex, 12].Value = record.WeightPerBox;
                        WorkSheet1.Cells[recordIndex, 13].Value = record.BoxCoverageAreaSqFoot;
                        WorkSheet1.Cells[recordIndex, 14].Value = record.BoxCoverageAreaSqMeter;
                        WorkSheet1.Cells[recordIndex, 15].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 16].Value = record.ValidationMessage;

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
                    WorkSheet1.Column(9).AutoFit();
                    WorkSheet1.Column(10).AutoFit();
                    WorkSheet1.Column(11).AutoFit();
                    WorkSheet1.Column(12).AutoFit();
                    WorkSheet1.Column(13).AutoFit();
                    WorkSheet1.Column(14).AutoFit();
                    WorkSheet1.Column(15).AutoFit();
                    WorkSheet1.Column(16).AutoFit();

                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadProductDesignTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.ProductDesignImportFormatFileName));
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
        public async Task<ResponseModel> ExportProductDesignListToExcel(ProductDesignSearchParameters request)
        {
            IEnumerable<ProductDesignResponse> productDesignResponse;

            request.IsExport = true;
            productDesignResponse = await _adminService.GetProductDesignList(request);
            if (productDesignResponse != null && productDesignResponse.ToList().Count > 0)
            {
                _response.Data = GenerateExcelProductDesignDataFile(productDesignResponse);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }

        private byte[] GenerateExcelProductDesignDataFile(IEnumerable<ProductDesignResponse> lstProductDesignToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"ProductDesign_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("ProductDesign");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Design Name";
                    excelWorksheet.Cells[1, 2].Value = "Size Name";
                    excelWorksheet.Cells[1, 3].Value = "Brand Name";
                    excelWorksheet.Cells[1, 4].Value = "Collection Name";
                    excelWorksheet.Cells[1, 5].Value = "Category Name";
                    excelWorksheet.Cells[1, 6].Value = "Type Name";
                    excelWorksheet.Cells[1, 7].Value = "Punch Name";
                    excelWorksheet.Cells[1, 8].Value = "Surface Name";
                    excelWorksheet.Cells[1, 9].Value = "Thickness Name";
                    excelWorksheet.Cells[1, 10].Value = "TileSize Name";
                    excelWorksheet.Cells[1, 11].Value = "No Of Tiles Per Box";
                    excelWorksheet.Cells[1, 12].Value = "Weight Per Box";
                    excelWorksheet.Cells[1, 13].Value = "Box Coverage Area SqFoot";
                    excelWorksheet.Cells[1, 14].Value = "Box Coverage Area SqMeter";
                    excelWorksheet.Cells[1, 15].Value = "IsActive";

                    recordIndex = 2;

                    foreach (ProductDesignResponse record in lstProductDesignToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.DesignName;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.SizeName;
                        excelWorksheet.Cells[recordIndex, 3].Value = record.BrandName;
                        excelWorksheet.Cells[recordIndex, 4].Value = record.CollectionName;
                        excelWorksheet.Cells[recordIndex, 5].Value = record.CategoryName;
                        excelWorksheet.Cells[recordIndex, 6].Value = record.TypeName;
                        excelWorksheet.Cells[recordIndex, 7].Value = record.PunchName;
                        excelWorksheet.Cells[recordIndex, 8].Value = record.SurfaceName;
                        excelWorksheet.Cells[recordIndex, 9].Value = record.ThicknessName;
                        excelWorksheet.Cells[recordIndex, 10].Value = record.TileSizeName;
                        excelWorksheet.Cells[recordIndex, 11].Value = record.NoOfTilesPerBox;
                        excelWorksheet.Cells[recordIndex, 12].Value = record.WeightPerBox;
                        excelWorksheet.Cells[recordIndex, 13].Value = record.BoxCoverageAreaSqFoot;
                        excelWorksheet.Cells[recordIndex, 14].Value = record.BoxCoverageAreaSqMeter;
                        excelWorksheet.Cells[recordIndex, 15].Value = record.IsActive;
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

                    excelData.SaveAs(msDataFile);
                    msDataFile.Position = 0;
                    result = msDataFile.ToArray();
                }
            }
            return result;
        }

    }
}
