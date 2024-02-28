﻿using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using static CasaAPI.Models.BrandModel;
using static CasaAPI.Models.CategoryModel;
using static CasaAPI.Models.CollectionModel;
using static CasaAPI.Models.PunchModel;
using static CasaAPI.Models.SurfaceModel;
using static CasaAPI.Models.ThicknessModel;
using static CasaAPI.Models.TileSizeModel;
using static CasaAPI.Models.TypeModel;
using static CasaAPI.Models.BloodModel;
using static CasaAPI.Models.TileTypeModel;
using static CasaAPI.Models.ContactTypeModel;
using static CasaAPI.Models.WeekCloseModel;
using static CasaAPI.Models.GendorModel;
using LicenseContext = OfficeOpenXml.LicenseContext;
using static CasaAPI.Models.CuttingSizeModel;

namespace CasaAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageProductController : CustomBaseController
    {
        private ResponseModel _response;
        private IAdminService _adminService;
        private IFileManager _fileManager;
        public ManageProductController(IAdminService adminService, IFileManager fileManager)
        {
            _adminService = adminService;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
        #region Size
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveSize(SizeSaveParameters Request)
        {
            int result = await _adminService.SaveSize(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Size Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Size details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSizesList(SizeSearchParameters request)
        {
            IEnumerable<SizeDetailsResponse> lstSizes = await _adminService.GetSizesList(request);
            _response.Data = lstSizes.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetSizeDetails(long id)
        {
            SizeDetailsResponse? size;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                size = await _adminService.GetSizeDetailsById(id);
                _response.Data = size;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadSizeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.SizeImportFormatFileName));
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
        public async Task<ResponseModel> ExportSizeListToExcel(SizeSearchParameters request)
        {
            IEnumerable<SizeDetailsResponse> sizeDetailsResponses;
            
            request.IsExport = true;
            sizeDetailsResponses = await _adminService.GetSizesList(request);
            if (sizeDetailsResponses != null && sizeDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelSizeDataFile(sizeDetailsResponses);
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
        public async Task<ResponseModel> ImportSizesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<SizeImportSaveParameters> lstSizeImportDetails = new List<SizeImportSaveParameters>();
            List<SizeFailToImportValidationErrors>? lstSizesFailedToImport = new List<SizeFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            SizeImportSaveParameters tempSizeImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Size data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "SizeName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempSizeImport = new SizeImportSaveParameters()
                    {
                        SizeName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempSizeImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstSizeImportDetails.Add(tempSizeImport);
                    }
                    else
                    {
                        lstSizesFailedToImport.Add(new SizeFailToImportValidationErrors()
                        {
                            SizeName = tempSizeImport.SizeName,
                            IsActive = tempSizeImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstSizeImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstSizesFailedToImport.AddRange(await _adminService.ImportSizesDetails(lstSizeImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Sizes list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstSizesFailedToImport != null && lstSizesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidSizeDataFile(lstSizesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidSizeDataFile(IEnumerable<SizeFailToImportValidationErrors> lstSizesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Size_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "SizeName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (SizeFailToImportValidationErrors record in lstSizesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.SizeName;
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
        private byte[] GenerateExcelSizeDataFile(IEnumerable<SizeDetailsResponse> lstSizesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Size_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Size");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Size Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (SizeDetailsResponse record in lstSizesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.SizeName;
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

        #region Brand
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBrand(BrandSaveParameters Request)
        {
            int result = await _adminService.SaveBrand(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Brand Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Brand details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBrandsList(BrandSearchParameters request)
        {
            IEnumerable<BrandDetailsResponse> lstBrands = await _adminService.GetBrandsList(request);
            _response.Data = lstBrands.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetBrandDetails(long id)
        {
            BrandDetailsResponse? brand;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                brand = await _adminService.GetBrandDetailsById(id);
                _response.Data = brand;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportBrandsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            List<string[]> data = new List<string[]>();
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<BrandImportSaveParameters> lstBrandImportDetails = new List<BrandImportSaveParameters>();
            List<BrandFailToImportValidationErrors>? lstBrandsFailedToImport = new List<BrandFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            BrandImportSaveParameters tempBrandImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Brand data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "BrandName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempBrandImport = new BrandImportSaveParameters()
                    {
                        BrandName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };
                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempBrandImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstBrandImportDetails.Add(tempBrandImport);
                    }
                    else
                    {
                        lstBrandsFailedToImport.Add(new BrandFailToImportValidationErrors()
                        {
                            BrandName = tempBrandImport.BrandName,
                            IsActive = tempBrandImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstBrandImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstBrandsFailedToImport.AddRange(await _adminService.ImportBrandsDetails(lstBrandImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Brands list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstBrandsFailedToImport != null && lstBrandsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidBrandDataFile(lstBrandsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidBrandDataFile(IEnumerable<BrandFailToImportValidationErrors> lstBrandsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Brand_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "BrandName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (BrandFailToImportValidationErrors record in lstBrandsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.BrandName;
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
        public async Task<ResponseModel> DownloadBrandTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.BrandImportFormatFileName));
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
        public async Task<ResponseModel> ExportBrandListToExcel(BrandSearchParameters request)
        {
            IEnumerable<BrandDetailsResponse> brandDetailsResponses;

            request.IsExport = true;
            brandDetailsResponses = await _adminService.GetBrandsList(request);
            if (brandDetailsResponses != null && brandDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelBrandDataFile(brandDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelBrandDataFile(IEnumerable<BrandDetailsResponse> lstBrandsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Brand_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Brand");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Brand Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (BrandDetailsResponse record in lstBrandsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.BrandName;
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

        #region Collection
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCollection(CollectionSaveParameters Request)
        {
            int result = await _adminService.SaveCollection(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Collection Name is already exists";
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
        public async Task<ResponseModel> GetCollectionsList(CollectionSearchParameters request)
        {
            IEnumerable<CollectionDetailsResponse> lstCollections = await _adminService.GetCollectionsList(request);
            _response.Data = lstCollections.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCollectionsDetails(long id)
        {
            CollectionDetailsResponse? collection;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                collection = await _adminService.GetCollectionDetailsById(id);
                _response.Data = collection;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportCollectionsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<CollectionImportSaveParameters> lstCollectionImportDetails = new List<CollectionImportSaveParameters>();
            List<CollectionFailToImportValidationErrors>? lstCollectionsFailedToImport = new List<CollectionFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            CollectionImportSaveParameters tempCollectionImport;

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
                    tempCollectionImport = new CollectionImportSaveParameters()
                    {
                        CollectionName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };
                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempCollectionImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstCollectionImportDetails.Add(tempCollectionImport);
                    }
                    else
                    {
                        lstCollectionsFailedToImport.Add(new CollectionFailToImportValidationErrors()
                        {
                            CollectionName = tempCollectionImport.CollectionName,
                            IsActive = tempCollectionImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstCollectionImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstCollectionsFailedToImport.AddRange(await _adminService.ImportCollectionsDetails(lstCollectionImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Collections list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstCollectionsFailedToImport != null && lstCollectionsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidCollectionDataFile(lstCollectionsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidCollectionDataFile(IEnumerable<CollectionFailToImportValidationErrors> lstCollectionsFailedToImport)
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

                    foreach (CollectionFailToImportValidationErrors record in lstCollectionsFailedToImport)
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
        [HttpGet]
        public async Task<ResponseModel> DownloadCollectionTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.CollectionImportFormatFileName));
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
        public async Task<ResponseModel> ExportCollectionListToExcel(CollectionSearchParameters request)
        {
            IEnumerable<CollectionDetailsResponse> collectionDetailsResponses;

            request.IsExport = true;
            collectionDetailsResponses = await _adminService.GetCollectionsList(request);
            if (collectionDetailsResponses != null && collectionDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelCollectionDataFile(collectionDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }
            return _response;
        }
        private byte[] GenerateExcelCollectionDataFile(IEnumerable<CollectionDetailsResponse> lstCollectionsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Collection_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Collection");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Collection Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (CollectionDetailsResponse record in lstCollectionsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.CollectionName;
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

        #region Category
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCategory(CategorySaveParameters Request)
        {
            int result = await _adminService.SaveCategory(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Category Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Category details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCategorysList(CategorySearchParameters request)
        {
            IEnumerable<CategoryDetailsResponse> lstCategorys = await _adminService.GetCategorysList(request);
            _response.Data = lstCategorys.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCategorysDetails(long id)
        {
            CategoryDetailsResponse? category;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                category = await _adminService.GetCategoryDetailsById(id);
                _response.Data = category;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportCategorysData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<CategoryImportSaveParameters> lstCategoryImportDetails = new List<CategoryImportSaveParameters>();
            List<CategoryFailToImportValidationErrors>? lstCategorysFailedToImport = new List<CategoryFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            CategoryImportSaveParameters tempCategoryImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Category data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "CategoryName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempCategoryImport = new CategoryImportSaveParameters()
                    {
                        CategoryName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempCategoryImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstCategoryImportDetails.Add(tempCategoryImport);
                    }
                    else
                    {
                        lstCategorysFailedToImport.Add(new CategoryFailToImportValidationErrors()
                        {
                            CategoryName = tempCategoryImport.CategoryName,
                            IsActive = tempCategoryImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstCategoryImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstCategorysFailedToImport.AddRange(await _adminService.ImportCategorysDetails(lstCategoryImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Categorys list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstCategorysFailedToImport != null && lstCategorysFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidCategoryDataFile(lstCategorysFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidCategoryDataFile(IEnumerable<CategoryFailToImportValidationErrors> lstCategorysFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Category_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CategoryName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (CategoryFailToImportValidationErrors record in lstCategorysFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.CategoryName;
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
        public async Task<ResponseModel> DownloadCategoryTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.CategoryImportFormatFileName));
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
        public async Task<ResponseModel> ExportCategoryListToExcel(CategorySearchParameters request)
        {
            IEnumerable<CategoryDetailsResponse> categoryDetailsResponses;

            request.IsExport = true;
            categoryDetailsResponses = await _adminService.GetCategorysList(request);
            if (categoryDetailsResponses != null && categoryDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelCategoryDataFile(categoryDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }
            return _response;
        }
        private byte[] GenerateExcelCategoryDataFile(IEnumerable<CategoryDetailsResponse> lstCategorysToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Category_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Category");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Category Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (CategoryDetailsResponse record in lstCategorysToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.CategoryName;
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

        #region Type
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveType(TypeSaveParameters Request)
        {
            int result = await _adminService.SaveType(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Type Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Type details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTypesList(TypeSearchParameters request)
        {
            IEnumerable<TypeDetailsResponse> lstTypes = await _adminService.GetTypesList(request);
            _response.Data = lstTypes.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetTypesDetails(long id)
        {
            TypeDetailsResponse? type;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                type = await _adminService.GetTypeDetailsById(id);
                _response.Data = type;
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportTypesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<TypeImportSaveParameters> lstTypeImportDetails = new List<TypeImportSaveParameters>();
            List<TypeFailToImportValidationErrors>? lstTypesFailedToImport = new List<TypeFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            TypeImportSaveParameters tempTypeImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "TypeName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempTypeImport = new TypeImportSaveParameters()
                    {
                        TypeName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempTypeImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstTypeImportDetails.Add(tempTypeImport);
                    }
                    else
                    {
                        lstTypesFailedToImport.Add(new TypeFailToImportValidationErrors()
                        {
                            TypeName = tempTypeImport.TypeName,
                            IsActive = tempTypeImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstTypeImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstTypesFailedToImport.AddRange(await _adminService.ImportTypesDetails(lstTypeImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Types list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstTypesFailedToImport != null && lstTypesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidTypeDataFile(lstTypesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidTypeDataFile(IEnumerable<TypeFailToImportValidationErrors> lstTypesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Type_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "TypeName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (TypeFailToImportValidationErrors record in lstTypesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.TypeName;
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
        public async Task<ResponseModel> DownloadTypeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.TypeImportFormatFileName));
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
        public async Task<ResponseModel> ExportTypeListToExcel(TypeSearchParameters request)
        {
            IEnumerable<TypeDetailsResponse> typeDetailsResponses;

            request.IsExport = true;
            typeDetailsResponses = await _adminService.GetTypesList(request);
            if (typeDetailsResponses != null && typeDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelTypeDataFile(typeDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }
            return _response;
        }
        private byte[] GenerateExcelTypeDataFile(IEnumerable<TypeDetailsResponse> lstTypesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Type_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Type");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Type Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (TypeDetailsResponse record in lstTypesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.TypeName;
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

        #region Punch
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePunch(PunchSaveParameters Request)
        {
            int result = await _adminService.SavePunch(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Punch Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Punch details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPunchsList(PunchSearchParameters request)
        {
            IEnumerable<PunchDetailsResponse> lstPunchs = await _adminService.GetPunchsList(request);
            _response.Data = lstPunchs.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPunchsDetails(long id)
        {
            PunchDetailsResponse? punch;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                punch = await _adminService.GetPunchDetailsById(id);
                _response.Data = punch;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportPunchsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<PunchImportSaveParameters> lstPunchImportDetails = new List<PunchImportSaveParameters>();
            List<PunchFailToImportValidationErrors> lstPunchsFailedToImport = new List<PunchFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            PunchImportSaveParameters tempPunchImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Punch data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "PunchName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempPunchImport = new PunchImportSaveParameters()
                    {
                        PunchName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempPunchImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstPunchImportDetails.Add(tempPunchImport);
                    }
                    else
                    {
                        lstPunchsFailedToImport.Add(new PunchFailToImportValidationErrors()
                        {
                            PunchName = tempPunchImport.PunchName,
                            IsActive = tempPunchImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstPunchImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstPunchsFailedToImport.AddRange(await _adminService.ImportPunchsDetails(lstPunchImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Punchs list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstPunchsFailedToImport != null && lstPunchsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidPunchDataFile(lstPunchsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidPunchDataFile(IEnumerable<PunchFailToImportValidationErrors> lstPunchsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Punch_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "PunchName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (PunchFailToImportValidationErrors record in lstPunchsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.PunchName;
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
        public async Task<ResponseModel> DownloadPunchTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.PunchImportFormatFileName));
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
        public async Task<ResponseModel> ExportPunchListToExcel(PunchSearchParameters request)
        {
            IEnumerable<PunchDetailsResponse> punchDetailsResponses;

            request.IsExport = true;
            punchDetailsResponses = await _adminService.GetPunchsList(request);
            if (punchDetailsResponses != null && punchDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelPunchDataFile(punchDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }
            return _response;
        }
        private byte[] GenerateExcelPunchDataFile(IEnumerable<PunchDetailsResponse> lstPunchsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Punch_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Punch");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Punch Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (PunchDetailsResponse record in lstPunchsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.PunchName;
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

        #region Surface
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveSurface(SurfaceSaveParameters Request)
        {
            int result = await _adminService.SaveSurface(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Surface Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Surface details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetSurfacesList(SurfaceSearchParameters request)
        {
            IEnumerable<SurfaceDetailsResponse> lstSurfaces = await _adminService.GetSurfacesList(request);
            _response.Data = lstSurfaces.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetSurfacesDetails(long id)
        {
            SurfaceDetailsResponse? surface;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                surface = await _adminService.GetSurfaceDetailsById(id);
                _response.Data = surface;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportSurfacesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<SurfaceImportSaveParameters> lstSurfaceImportDetails = new List<SurfaceImportSaveParameters>();
            List<SurfaceFailToImportValidationErrors> lstSurfacesFailedToImport = new List<SurfaceFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            SurfaceImportSaveParameters tempSurfaceImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Surface data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "SurfaceName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempSurfaceImport = new SurfaceImportSaveParameters()
                    {
                        SurfaceName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempSurfaceImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstSurfaceImportDetails.Add(tempSurfaceImport);
                    }
                    else
                    {
                        lstSurfacesFailedToImport.Add(new SurfaceFailToImportValidationErrors()
                        {
                            SurfaceName = tempSurfaceImport.SurfaceName,
                            IsActive = tempSurfaceImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstSurfaceImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstSurfacesFailedToImport.AddRange(await _adminService.ImportSurfacesDetails(lstSurfaceImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Surfaces list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstSurfacesFailedToImport != null && lstSurfacesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidSurfaceDataFile(lstSurfacesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidSurfaceDataFile(IEnumerable<SurfaceFailToImportValidationErrors> lstSurfacesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Surface_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "SurfaceName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (SurfaceFailToImportValidationErrors record in lstSurfacesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.SurfaceName;
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
        public async Task<ResponseModel> DownloadSurfaceTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.SurfaceImportFormatFileName));
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
        public async Task<ResponseModel> ExportSurfaceListToExcel(SurfaceSearchParameters request)
        {
            IEnumerable<SurfaceDetailsResponse> surfaceDetailsResponses;

            request.IsExport = true;
            surfaceDetailsResponses = await _adminService.GetSurfacesList(request);
            if (surfaceDetailsResponses != null && surfaceDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelSurfaceDataFile(surfaceDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }
            return _response;
        }
        private byte[] GenerateExcelSurfaceDataFile(IEnumerable<SurfaceDetailsResponse> lstSurfacesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Surface_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Surface");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Surface Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (SurfaceDetailsResponse record in lstSurfacesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.SurfaceName;
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

        #region Thickness
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveThickness(ThicknessSaveParameters Request)
        {
            int result = await _adminService.SaveThickness(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Thickness Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Thickness details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetThicknessesList(ThicknessSearchParameters request)
        {
            IEnumerable<ThicknessDetailsResponse> lstThicknesses = await _adminService.GetThicknessesList(request);
            _response.Data = lstThicknesses.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetThicknessesDetails(long id)
        {
            ThicknessDetailsResponse? thickness;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                thickness = await _adminService.GetThicknessDetailsById(id);
                _response.Data = thickness;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportThicknessesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ThicknessImportSaveParameters> lstThicknessImportDetails = new List<ThicknessImportSaveParameters>();
            List<ThicknessFailToImportValidationErrors>? lstThicknessFailedToImport = new List<ThicknessFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            ThicknessImportSaveParameters tempThicknessImport;


            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Thickness data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "ThicknessName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempThicknessImport = new ThicknessImportSaveParameters()
                    {
                        ThicknessName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempThicknessImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstThicknessImportDetails.Add(tempThicknessImport);
                    }
                    else
                    {
                        lstThicknessFailedToImport.Add(new ThicknessFailToImportValidationErrors()
                        {
                            ThicknessName = tempThicknessImport.ThicknessName,
                            IsActive = tempThicknessImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstThicknessImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstThicknessFailedToImport.AddRange(await _adminService.ImportThicknessesDetails(lstThicknessImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Thicknesses list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstThicknessFailedToImport != null && lstThicknessFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidThicknessDataFile(lstThicknessFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidThicknessDataFile(IEnumerable<ThicknessFailToImportValidationErrors> lstThicknessFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Thickness_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "ThicknessName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (ThicknessFailToImportValidationErrors record in lstThicknessFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.ThicknessName;
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
        public async Task<ResponseModel> DownloadThicknessTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.ThicknessImportFormatFileName));
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
        public async Task<ResponseModel> ExportThicknessListToExcel(ThicknessSearchParameters request)
        {
            IEnumerable<ThicknessDetailsResponse> thicknessDetailsResponses;

            request.IsExport = true;
            thicknessDetailsResponses = await _adminService.GetThicknessesList(request);
            if (thicknessDetailsResponses != null && thicknessDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelThicknessDataFile(thicknessDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }
            return _response;
        }
        private byte[] GenerateExcelThicknessDataFile(IEnumerable<ThicknessDetailsResponse> lstThicknessToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Thickness_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Thickness");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Thickness Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (ThicknessDetailsResponse record in lstThicknessToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.ThicknessName;
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

        #region TileSize
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveTileSize(TileSizeSaveParameters Request)
        {
            int result = await _adminService.SaveTileSize(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "TileSize Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "TileSize details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTileSizesList(TileSizeSearchParameters request)
        {
            IEnumerable<TileSizeDetailsResponse> lstTileSizes = await _adminService.GetTileSizesList(request);
            _response.Data = lstTileSizes.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetTileSizesDetails(long id)
        {
            TileSizeDetailsResponse? tileSize;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                tileSize = await _adminService.GetTileSizeDetailsById(id);
                _response.Data = tileSize;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportTileSizesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<TileSizeImportSaveParameters> lstTileSizeImportDetails = new List<TileSizeImportSaveParameters>();
            List<TileSizeFailToImportValidationErrors>? lstTileSizesFailedToImport = new List<TileSizeFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            TileSizeImportSaveParameters tempTileSizeImport;


            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import TileSize data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "TileSizeName", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempTileSizeImport = new TileSizeImportSaveParameters()
                    {
                        TileSizeName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempTileSizeImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstTileSizeImportDetails.Add(tempTileSizeImport);
                    }
                    else
                    {
                        lstTileSizesFailedToImport.Add(new TileSizeFailToImportValidationErrors()
                        {
                            TileSizeName = tempTileSizeImport.TileSizeName,
                            IsActive = tempTileSizeImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstTileSizeImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstTileSizesFailedToImport.AddRange(await _adminService.ImportTileSizesDetails(lstTileSizeImportDetails));

            _response.IsSuccess = true;
            _response.Message = "TileSizes list imported successfully";

            #region Generate Excel file for Invalid Data
            if (lstTileSizesFailedToImport != null && lstTileSizesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidTileSizeDataFile(lstTileSizesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidTileSizeDataFile(IEnumerable<TileSizeFailToImportValidationErrors> lstTileSizesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_TileSize_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "TileSizeName";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (TileSizeFailToImportValidationErrors record in lstTileSizesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.TileSizeName;
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
        public async Task<ResponseModel> DownloadTileSizeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.TileSizeImportFormatFileName));
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
        public async Task<ResponseModel> ExportTileSizeListToExcel(TileSizeSearchParameters request)
        {
            IEnumerable<TileSizeDetailsResponse> tileSizeDetailsResponses;

            request.IsExport = true;
            tileSizeDetailsResponses = await _adminService.GetTileSizesList(request);
            if (tileSizeDetailsResponses != null && tileSizeDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelTileSizeDataFile(tileSizeDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }
            return _response;
        }
        private byte[] GenerateExcelTileSizeDataFile(IEnumerable<TileSizeDetailsResponse> lstTileSizesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"TileSize_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("TileSize");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Tile Size Name";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (TileSizeDetailsResponse record in lstTileSizesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.TileSizeName;
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

        #region Blood
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBlood(BloodSaveParameters Request)
        {
            int result = await _adminService.SaveBlood(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Blood Group is already exists";
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
        public async Task<ResponseModel> GetBloodList(BloodSearchParameters request)
        {
            IEnumerable<BloodDetailsResponse> lstBloods = await _adminService.GetBloodsList(request);
            _response.Data = lstBloods.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetBloodDetails(long id)
        {
            BloodDetailsResponse? Blood;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                Blood = await _adminService.GetBloodDetailsById(id);
                _response.Data = Blood;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportBloodsData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<BloodImportSaveParameters> lstBloodImportDetails = new List<BloodImportSaveParameters>();
            List<BloodFailToImportValidationErrors>? lstBloodsFailedToImport = new List<BloodFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            BloodImportSaveParameters tempBloodImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Blood data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "BloodGroup", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempBloodImport = new BloodImportSaveParameters()
                    {
                        BloodGroup = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempBloodImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstBloodImportDetails.Add(tempBloodImport);
                    }
                    else
                    {
                        lstBloodsFailedToImport.Add(new BloodFailToImportValidationErrors()
                        {
                            BloodGroup = tempBloodImport.BloodGroup,
                            IsActive = tempBloodImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstBloodImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstBloodsFailedToImport.AddRange(await _adminService.ImportBloodsDetails(lstBloodImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Bloods list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstBloodsFailedToImport != null && lstBloodsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidBloodDataFile(lstBloodsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidBloodDataFile(IEnumerable<BloodFailToImportValidationErrors> lstBloodsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Blood_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "BloodGroup";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (BloodFailToImportValidationErrors record in lstBloodsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.BloodGroup;
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
        public async Task<ResponseModel> DownloadBloodTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.BloodImportFormatFileName));
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
        public async Task<ResponseModel> ExportBloodListToExcel(BloodSearchParameters request)
        {
            IEnumerable<BloodDetailsResponse> BloodDetailsResponses;

            request.IsExport = true;
            BloodDetailsResponses = await _adminService.GetBloodsList(request);
            if (BloodDetailsResponses != null && BloodDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelBloodDataFile(BloodDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelBloodDataFile(IEnumerable<BloodDetailsResponse> lstBloodsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Blood_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Blood");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Blood Group";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (BloodDetailsResponse record in lstBloodsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.BloodGroup;
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

        #region TileType
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveTileType(TileTypeSaveParameters Request)
        {
            int result = await _adminService.SaveTileType(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Tile Type is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Tile Type details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetTileTypeList(TileTypeSearchParameters request)
        {
            IEnumerable<TileTypeDetailsResponse> lstTileTypes = await _adminService.GetTileTypesList(request);
            _response.Data = lstTileTypes.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetTileTypeDetails(long id)
        {
            TileTypeDetailsResponse? TileType;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                TileType = await _adminService.GetTileTypeDetailsById(id);
                _response.Data = TileType;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportTileTypesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<TileTypeImportSaveParameters> lstTileTypeImportDetails = new List<TileTypeImportSaveParameters>();
            List<TileTypeFailToImportValidationErrors>? lstTileTypesFailedToImport = new List<TileTypeFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            TileTypeImportSaveParameters tempTileTypeImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Tile Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "TileType", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempTileTypeImport = new TileTypeImportSaveParameters()
                    {
                        TileType = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempTileTypeImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstTileTypeImportDetails.Add(tempTileTypeImport);
                    }
                    else
                    {
                        lstTileTypesFailedToImport.Add(new TileTypeFailToImportValidationErrors()
                        {
                            TileType = tempTileTypeImport.TileType,
                            IsActive = tempTileTypeImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstTileTypeImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstTileTypesFailedToImport.AddRange(await _adminService.ImportTileTypesDetails(lstTileTypeImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Tile Types list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstTileTypesFailedToImport != null && lstTileTypesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidTileTypeDataFile(lstTileTypesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidTileTypeDataFile(IEnumerable<TileTypeFailToImportValidationErrors> lstTileTypesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_TileType_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "TileType";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (TileTypeFailToImportValidationErrors record in lstTileTypesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.TileType;
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
        public async Task<ResponseModel> DownloadTileTypeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.TileTypeImportFormatFileName));
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
        public async Task<ResponseModel> ExportTileTypeListToExcel(TileTypeSearchParameters request)
        {
            IEnumerable<TileTypeDetailsResponse> TileTypeDetailsResponses;

            request.IsExport = true;
            TileTypeDetailsResponses = await _adminService.GetTileTypesList(request);
            if (TileTypeDetailsResponses != null && TileTypeDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelTileTypeDataFile(TileTypeDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelTileTypeDataFile(IEnumerable<TileTypeDetailsResponse> lstTileTypesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"TileType_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("TileType");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Tile Type";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (TileTypeDetailsResponse record in lstTileTypesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.TileType;
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

        #region ContactType
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveContactType(ContactTypeSaveParameters Request)
        {
            int result = await _adminService.SaveContactType(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Contact Type is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Contact Type details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetContactTypeList(ContactTypeSearchParameters request)
        {
            IEnumerable<ContactTypeDetailsResponse> lstContactTypes = await _adminService.GetContactTypesList(request);
            _response.Data = lstContactTypes.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetContactTypeDetails(long id)
        {
            ContactTypeDetailsResponse? ContactType;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                ContactType = await _adminService.GetContactTypeDetailsById(id);
                _response.Data = ContactType;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportContactTypesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ContactTypeImportSaveParameters> lstContactTypeImportDetails = new List<ContactTypeImportSaveParameters>();
            List<ContactTypeFailToImportValidationErrors>? lstContactTypesFailedToImport = new List<ContactTypeFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            ContactTypeImportSaveParameters tempContactTypeImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Contact Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "ContactType", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempContactTypeImport = new ContactTypeImportSaveParameters()
                    {
                        ContactType = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempContactTypeImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstContactTypeImportDetails.Add(tempContactTypeImport);
                    }
                    else
                    {
                        lstContactTypesFailedToImport.Add(new ContactTypeFailToImportValidationErrors()
                        {
                            ContactType = tempContactTypeImport.ContactType,
                            IsActive = tempContactTypeImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstContactTypeImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstContactTypesFailedToImport.AddRange(await _adminService.ImportContactTypesDetails(lstContactTypeImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Contact Types list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstContactTypesFailedToImport != null && lstContactTypesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidContactTypeDataFile(lstContactTypesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidContactTypeDataFile(IEnumerable<ContactTypeFailToImportValidationErrors> lstContactTypesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_ContactType_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "ContactType";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (ContactTypeFailToImportValidationErrors record in lstContactTypesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.ContactType;
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
        public async Task<ResponseModel> DownloadContactTypeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.ContactTypeImportFormatFileName));
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
        public async Task<ResponseModel> ExportContactTypeListToExcel(ContactTypeSearchParameters request)
        {
            IEnumerable<ContactTypeDetailsResponse> ContactTypeDetailsResponses;

            request.IsExport = true;
            ContactTypeDetailsResponses = await _adminService.GetContactTypesList(request);
            if (ContactTypeDetailsResponses != null && ContactTypeDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelContactTypeDataFile(ContactTypeDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelContactTypeDataFile(IEnumerable<ContactTypeDetailsResponse> lstContactTypesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"ContactType_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("ContactType");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Contact Type";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (ContactTypeDetailsResponse record in lstContactTypesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.ContactType;
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

        #region WeekClose
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveWeekClose(WeekCloseSaveParameters Request)
        {
            int result = await _adminService.SaveWeekClose(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Week Close is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Week Close details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetWeekCloseList(WeekCloseSearchParameters request)
        {
            IEnumerable<WeekCloseDetailsResponse> lstWeekCloses = await _adminService.GetWeekClosesList(request);
            _response.Data = lstWeekCloses.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetWeekCloseDetails(long id)
        {
            WeekCloseDetailsResponse? WeekClose;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                WeekClose = await _adminService.GetWeekCloseDetailsById(id);
                _response.Data = WeekClose;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportWeekClosesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<WeekCloseImportSaveParameters> lstWeekCloseImportDetails = new List<WeekCloseImportSaveParameters>();
            List<WeekCloseFailToImportValidationErrors>? lstWeekClosesFailedToImport = new List<WeekCloseFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            WeekCloseImportSaveParameters tempWeekCloseImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Week Close data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "WeekClose", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempWeekCloseImport = new WeekCloseImportSaveParameters()
                    {
                        WeekClose = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempWeekCloseImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstWeekCloseImportDetails.Add(tempWeekCloseImport);
                    }
                    else
                    {
                        lstWeekClosesFailedToImport.Add(new WeekCloseFailToImportValidationErrors()
                        {
                            WeekClose = tempWeekCloseImport.WeekClose,
                            IsActive = tempWeekCloseImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstWeekCloseImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstWeekClosesFailedToImport.AddRange(await _adminService.ImportWeekClosesDetails(lstWeekCloseImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Week Closes list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstWeekClosesFailedToImport != null && lstWeekClosesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidWeekCloseDataFile(lstWeekClosesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidWeekCloseDataFile(IEnumerable<WeekCloseFailToImportValidationErrors> lstWeekClosesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_WeekClose_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "WeekClose";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (WeekCloseFailToImportValidationErrors record in lstWeekClosesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.WeekClose;
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
        public async Task<ResponseModel> DownloadWeekCloseTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.WeekCloseImportFormatFileName));
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
        public async Task<ResponseModel> ExportWeekCloseListToExcel(WeekCloseSearchParameters request)
        {
            IEnumerable<WeekCloseDetailsResponse> WeekCloseDetailsResponses;

            request.IsExport = true;
            WeekCloseDetailsResponses = await _adminService.GetWeekClosesList(request);
            if (WeekCloseDetailsResponses != null && WeekCloseDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelWeekCloseDataFile(WeekCloseDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelWeekCloseDataFile(IEnumerable<WeekCloseDetailsResponse> lstWeekClosesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"WeekClose_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("WeekClose");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Week Close";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (WeekCloseDetailsResponse record in lstWeekClosesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.WeekClose;
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

        #region Gender
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveGender(GendorSaveParameters Request)
        {
            int result = await _adminService.SaveGendor(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Gendor is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Gendor details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetGenderList(GendorSearchParameters request)
        {
            IEnumerable<GendorDetailsResponse> lstGendors = await _adminService.GetGendorsList(request);
            _response.Data = lstGendors.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetGenderDetails(long id)
        {
            GendorDetailsResponse? Gendor;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                Gendor = await _adminService.GetGendorDetailsById(id);
                _response.Data = Gendor;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportGendersData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<GendorImportSaveParameters> lstGendorImportDetails = new List<GendorImportSaveParameters>();
            List<GendorFailToImportValidationErrors>? lstGendorsFailedToImport = new List<GendorFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            GendorImportSaveParameters tempGendorImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Contact Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "Gender", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempGendorImport = new GendorImportSaveParameters()
                    {
                        Gender = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempGendorImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstGendorImportDetails.Add(tempGendorImport);
                    }
                    else
                    {
                        lstGendorsFailedToImport.Add(new GendorFailToImportValidationErrors()
                        {
                            Gender = tempGendorImport.Gender,
                            IsActive = tempGendorImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstGendorImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstGendorsFailedToImport.AddRange(await _adminService.ImportGendorsDetails(lstGendorImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Genders list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstGendorsFailedToImport != null && lstGendorsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidGendorDataFile(lstGendorsFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidGendorDataFile(IEnumerable<GendorFailToImportValidationErrors> lstGendorsFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Gendor_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "Gendor";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (GendorFailToImportValidationErrors record in lstGendorsFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.Gender;
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
        public async Task<ResponseModel> DownloadGenderTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.GendorImportFormatFileName));
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
        public async Task<ResponseModel> ExportGenderListToExcel(GendorSearchParameters request)
        {
            IEnumerable<GendorDetailsResponse> GendorDetailsResponses;

            request.IsExport = true;
            GendorDetailsResponses = await _adminService.GetGendorsList(request);
            if (GendorDetailsResponses != null && GendorDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelGendorDataFile(GendorDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelGendorDataFile(IEnumerable<GendorDetailsResponse> lstGendorsToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Gendor_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Gendor");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Gender";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (GendorDetailsResponse record in lstGendorsToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.Gender;
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
        #region CuttingSize
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCuttingSize(CuttingSizeSaveParameters Request)
        {
            int result = await _adminService.SaveCuttingSize(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Cutting Size is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Cutting Size details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCuttingSizeList(CuttingSizeSearchParameters request)
        {
            IEnumerable<CuttingSizeDetailsResponse> lstCuttingSizes = await _adminService.GetCuttingSizesList(request);
            _response.Data = lstCuttingSizes.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCuttingSizeDetails(long id)
        {
            CuttingSizeDetailsResponse? CuttingSize;
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                CuttingSize = await _adminService.GetCuttingSizeDetailsById(id);
                _response.Data = CuttingSize;
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> ImportCuttingSizesData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<CuttingSizeImportSaveParameters> lstCuttingSizeImportDetails = new List<CuttingSizeImportSaveParameters>();
            List<CuttingSizeFailToImportValidationErrors>? lstCuttingSizesFailedToImport = new List<CuttingSizeFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            CuttingSizeImportSaveParameters tempCuttingSizeImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Contact Type data";
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

                if (!string.Equals(workSheet.Cells[1, 1].Value.ToString(), "CuttingSize", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 2].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempCuttingSizeImport = new CuttingSizeImportSaveParameters()
                    {
                        CuttingSize = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 2].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempCuttingSizeImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstCuttingSizeImportDetails.Add(tempCuttingSizeImport);
                    }
                    else
                    {
                        lstCuttingSizesFailedToImport.Add(new CuttingSizeFailToImportValidationErrors()
                        {
                            CuttingSize = tempCuttingSizeImport.CuttingSize,
                            IsActive = tempCuttingSizeImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstCuttingSizeImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstCuttingSizesFailedToImport.AddRange(await _adminService.ImportCuttingSizeDetails(lstCuttingSizeImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Cutting Sizes list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstCuttingSizesFailedToImport != null && lstCuttingSizesFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidCuttingSizeDataFile(lstCuttingSizesFailedToImport);

            }
            #endregion

            return _response;
        }
        private byte[] GenerateInvalidCuttingSizeDataFile(IEnumerable<CuttingSizeFailToImportValidationErrors> lstCuttingSizesFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_CuttingSize_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "CuttingSize";
                    WorkSheet1.Cells[1, 2].Value = "IsActive";
                    WorkSheet1.Cells[1, 3].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (CuttingSizeFailToImportValidationErrors record in lstCuttingSizesFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.CuttingSize;
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
        public async Task<ResponseModel> DownloadCuttingSizeTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.CuttingSizeImportFormatFileName));
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
        public async Task<ResponseModel> ExportCuttingSizeListToExcel(CuttingSizeSearchParameters request)
        {
            IEnumerable<CuttingSizeDetailsResponse> CuttingSizeDetailsResponses;

            request.IsExport = true;
            CuttingSizeDetailsResponses = await _adminService.GetCuttingSizesList(request);
            if (CuttingSizeDetailsResponses != null && CuttingSizeDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelCuttingSizeDataFile(CuttingSizeDetailsResponses);
            }
            else
            {
                _response.Message = "Record Not Exists";
                _response.IsSuccess = false;
            }

            return _response;
        }
        private byte[] GenerateExcelCuttingSizeDataFile(IEnumerable<CuttingSizeDetailsResponse> lstCuttingSizesToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"CuttingSize_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("CuttingSize");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "Cutting Size";
                    excelWorksheet.Cells[1, 2].Value = "Is Active?";

                    recordIndex = 2;

                    foreach (CuttingSizeDetailsResponse record in lstCuttingSizesToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.CuttingSize;
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
    }
}
