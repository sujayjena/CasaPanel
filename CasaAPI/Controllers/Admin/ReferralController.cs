using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace CasaAPI.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferralController : ControllerBase
    {
        private ResponseModel _response;
        private IAdminService _adminService;
        private IFileManager _fileManager;
        public ReferralController(IAdminService adminService, IFileManager fileManager)
        {
            _adminService = adminService;
            _fileManager = fileManager;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
        #region Referral
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveReferral([FromForm] ReferralSaveParameters parameter)
        {
            int result;
            List<ResponseModel> lstValidationResponse = new List<ResponseModel>();
            ResponseModel? validationResponse;


            if (HttpContext.Request.Form.Files.Count > 0)
            {
                if (string.IsNullOrEmpty(parameter.AadharSaveFileName))
                {
                    parameter.AadharFile = HttpContext.Request.Form.Files["AadharFile"];
                    parameter.AadharFileName = parameter.AadharFile?.FileName;
                }

                if (string.IsNullOrEmpty(parameter.PanCardSaveFileName))
                {
                    parameter.PanCardFile = HttpContext.Request.Form.Files["PanCardFile"];
                    parameter.PanCardFileName = parameter.PanCardFile?.FileName;
                }
            }

            //To validate Main object
            lstValidationResponse.Add(ModelStateHelper.GetValidationErrorsList(parameter));

            validationResponse = lstValidationResponse.Where(v => v.IsSuccess == false).FirstOrDefault();

            if (validationResponse != null && validationResponse.IsSuccess == false)
            {
                return validationResponse;
            }

            if (parameter.AadharFile != null)
                parameter.AadharSaveFileName = _fileManager.UploadReferralDocuments(parameter.AadharFile);

            if (parameter.PanCardFile != null)
                parameter.PanCardSaveFileName = _fileManager.UploadReferralDocuments(parameter.PanCardFile);

            result = await _adminService.SaveReferral(parameter);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Referral Name is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Referral details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetReferralList(ReferralSearchParameters request)
        {
            IEnumerable<ReferralDetailsResponse> lstReferral = await _adminService.GetReferralList(request);
            _response.Data = lstReferral.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetReferralDetails(long id)
        {
            var host = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            ReferralDetailsResponse? referral;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                referral = await _adminService.GetReferralDetailsById(id);
                if (referral != null)
                {
                    if (!string.IsNullOrWhiteSpace(referral.AadharSaveFileName))
                    {
                        referral.AadharFileUrl = host + _fileManager.GetReferralDocumentsFile(referral.AadharSaveFileName);
                    }

                    if (!string.IsNullOrWhiteSpace(referral.PanCardSaveFileName))
                    {
                        referral.PanCardFileUrl = host + _fileManager.GetReferralDocumentsFile(referral.PanCardSaveFileName);
                    }
                }
                _response.Data = referral;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> DownloadReferralTemplate()
        {
            byte[]? fileContent = await Task.Run(() => _fileManager.GetFormatFileFromPath(FormatFilesName.ReferralImportFormatFileName));
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
        public async Task<ResponseModel> ExportReferralListToExcel(ReferralSearchParameters request)
        {
            IEnumerable<ReferralDetailsResponse> referralDetailsResponses;

            request.IsExport = true;
            referralDetailsResponses = await _adminService.GetReferralList(request);
            if (referralDetailsResponses != null && referralDetailsResponses.ToList().Count > 0)
            {
                _response.Data = GenerateExcelReferralDataFile(referralDetailsResponses);
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
        public async Task<ResponseModel> ImportReferralData([FromQuery] ImportRequest request)
        {
            _response.IsSuccess = false;
            ExcelWorksheets currentSheet;
            ExcelWorksheet workSheet;
            List<ReferralImportSaveParameters> lstReferralImportDetails = new List<ReferralImportSaveParameters>();
            List<ReferralFailToImportValidationErrors>? lstReferralsFailedToImport = new List<ReferralFailToImportValidationErrors>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            int noOfCol, noOfRow;
            ResponseModel? fileDataValidationRes;
            ReferralImportSaveParameters tempReferralImport;

            if (request.FileUpload == null || request.FileUpload.Length == 0)
            {
                _response.Message = "Please upload an excel file to import Referral data";
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

                if (!string.Equals(workSheet.Cells[1, 2].Value.ToString(), "ReferralParty", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 3].Value.ToString(), "Address", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 4].Value.ToString(), "StateName", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 5].Value.ToString(), "RegionName", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 6].Value.ToString(), "DistrictName", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 7].Value.ToString(), "CityName", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(workSheet.Cells[1, 8].Value.ToString(), "AreaName", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 9].Value.ToString(), "Pincode", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 10].Value.ToString(), "Phone", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 11].Value.ToString(), "Mobile", StringComparison.OrdinalIgnoreCase) ||
                      !string.Equals(workSheet.Cells[1, 12].Value.ToString(), "GstNo", StringComparison.OrdinalIgnoreCase) ||
                     !string.Equals(workSheet.Cells[1, 13].Value.ToString(), "PanNo", StringComparison.OrdinalIgnoreCase) ||
                   !string.Equals(workSheet.Cells[1, 14].Value.ToString(), "IsActive", StringComparison.OrdinalIgnoreCase))
                {
                    _response.IsSuccess = false;
                    _response.Message = "Please upload a valid excel file. Please Download Format file for reference";
                    return _response;
                }

                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    tempReferralImport = new ReferralImportSaveParameters()
                    {
                        UniqueNo = _adminService.RandomDigits(10),
                        ReferralParty = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                        Address = workSheet.Cells[rowIterator, 2].Value?.ToString(),
                        StateName = workSheet.Cells[rowIterator, 3].Value?.ToString(),
                        RegionName = workSheet.Cells[rowIterator, 4].Value?.ToString(),
                        DistrictName = workSheet.Cells[rowIterator, 5].Value?.ToString(),
                        CityName = workSheet.Cells[rowIterator, 6].Value?.ToString(),
                        AreaName = workSheet.Cells[rowIterator, 7].Value?.ToString(),
                        Pincode = workSheet.Cells[rowIterator, 8].Value?.ToString(),
                        Phone = workSheet.Cells[rowIterator, 9].Value?.ToString(),
                        Mobile = workSheet.Cells[rowIterator, 10].Value?.ToString(),
                        GstNo = workSheet.Cells[rowIterator, 11].Value?.ToString(),
                        PanNo = workSheet.Cells[rowIterator, 12].Value?.ToString(),
                        IsActive = workSheet.Cells[rowIterator, 13].Value?.ToString()
                    };

                    fileDataValidationRes = ModelStateHelper.GetValidationErrorsList(model: tempReferralImport);

                    if (fileDataValidationRes.IsSuccess)
                    {
                        lstReferralImportDetails.Add(tempReferralImport);
                    }
                    else
                    {
                        lstReferralsFailedToImport.Add(new ReferralFailToImportValidationErrors()
                        {
                            UniqueNo = tempReferralImport.UniqueNo,
                            ReferralParty = tempReferralImport.ReferralParty,
                            Address = tempReferralImport.Address,
                            StateName = tempReferralImport.StateName,
                            RegionName = tempReferralImport.RegionName,
                            DistrictName = tempReferralImport.DistrictName,
                            AreaName = tempReferralImport.AreaName,
                            Pincode = tempReferralImport.Pincode,
                            Phone = tempReferralImport.Phone,
                            Mobile = tempReferralImport.Mobile,
                            GstNo = tempReferralImport.GstNo,
                            PanNo = tempReferralImport.PanNo,
                            CityName = tempReferralImport.CityName,
                            IsActive = tempReferralImport.IsActive,
                            ValidationMessage = fileDataValidationRes.Message
                        });
                    }
                }
            }

            if (lstReferralImportDetails.Count == 0)
            {
                _response.Message = "File does not contains any record(s)";
                return _response;
            }

            lstReferralsFailedToImport.AddRange(await _adminService.ImportReferralDetails(lstReferralImportDetails));

            _response.IsSuccess = true;
            _response.Message = "Referral list imported successfully";

            #region Generate Excel file for Invalid Data

            if (lstReferralsFailedToImport != null && lstReferralsFailedToImport.ToList().Count > 0)
            {
                _response.Message = "Uploaded file contains invalid records, please check downloaded file for more details";
                _response.Data = GenerateInvalidReferralDataFile(lstReferralsFailedToImport);

            }
            #endregion

            return _response;
        }

       
        private byte[] GenerateInvalidReferralDataFile(IEnumerable<ReferralFailToImportValidationErrors> lstReferralFailedToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet WorkSheet1;

            using (MemoryStream msInvalidDataFile = new MemoryStream())
            {
                using (ExcelPackage excelInvalidData = new ExcelPackage())
                {
                    WorkSheet1 = excelInvalidData.Workbook.Worksheets.Add("Invalid_Referral_Records");
                    WorkSheet1.TabColor = System.Drawing.Color.Black;
                    WorkSheet1.DefaultRowHeight = 12;

                    //Header of table
                    WorkSheet1.Row(1).Height = 20;
                    WorkSheet1.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    WorkSheet1.Row(1).Style.Font.Bold = true;

                    WorkSheet1.Cells[1, 1].Value = "UniqueNo";
                    WorkSheet1.Cells[1, 2].Value = "ReferralParty";
                    WorkSheet1.Cells[1, 3].Value = "Address";
                    WorkSheet1.Cells[1, 4].Value = "StateName";
                    WorkSheet1.Cells[1, 5].Value = "RegionName";
                    WorkSheet1.Cells[1, 6].Value = "DistrictName";
                    WorkSheet1.Cells[1, 7].Value = "CityName";
                    WorkSheet1.Cells[1, 8].Value = "AreaName";
                    WorkSheet1.Cells[1, 9].Value = "Pincode";
                    WorkSheet1.Cells[1, 10].Value = "Phone";
                    WorkSheet1.Cells[1, 11].Value = "Mobile";
                    WorkSheet1.Cells[1, 12].Value = "GstNo";
                    WorkSheet1.Cells[1, 13].Value = "PanNo";
                    WorkSheet1.Cells[1, 14].Value = "IsActive";
                    WorkSheet1.Cells[1, 15].Value = "ValidationMessage";

                    recordIndex = 2;

                    foreach (ReferralFailToImportValidationErrors record in lstReferralFailedToImport)
                    {
                        WorkSheet1.Cells[recordIndex, 1].Value = record.UniqueNo;
                        WorkSheet1.Cells[recordIndex, 2].Value = record.ReferralParty;
                        WorkSheet1.Cells[recordIndex, 3].Value = record.Address;
                        WorkSheet1.Cells[recordIndex, 4].Value = record.StateName;
                        WorkSheet1.Cells[recordIndex, 5].Value = record.RegionName;
                        WorkSheet1.Cells[recordIndex, 6].Value = record.DistrictName;
                        WorkSheet1.Cells[recordIndex, 7].Value = record.CityName;
                        WorkSheet1.Cells[recordIndex, 8].Value = record.AreaName;
                        WorkSheet1.Cells[recordIndex, 9].Value = record.Pincode;
                        WorkSheet1.Cells[recordIndex, 10].Value = record.Phone;
                        WorkSheet1.Cells[recordIndex, 11].Value = record.Mobile;
                        WorkSheet1.Cells[recordIndex, 12].Value = record.GstNo;
                        WorkSheet1.Cells[recordIndex, 13].Value = record.PanNo;
                        WorkSheet1.Cells[recordIndex, 14].Value = record.IsActive;
                        WorkSheet1.Cells[recordIndex, 15].Value = record.ValidationMessage;

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
                    excelInvalidData.SaveAs(msInvalidDataFile);
                    msInvalidDataFile.Position = 0;
                    result = msInvalidDataFile.ToArray();
                }
            }

            return result;
        }
        private byte[] GenerateExcelReferralDataFile(IEnumerable<ReferralDetailsResponse> lstReferralToImport)
        {
            byte[] result;
            int recordIndex;
            ExcelWorksheet excelWorksheet;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (MemoryStream msDataFile = new MemoryStream())
            {
                using (ExcelPackage excelData = new ExcelPackage(new FileInfo($"Referral_List_{DateTime.Now.ToString("yyyyMMddHHmm")}")))
                {
                    excelWorksheet = excelData.Workbook.Worksheets.Add("Referral");
                    excelWorksheet.TabColor = System.Drawing.Color.Black;
                    excelWorksheet.DefaultRowHeight = 12;

                    //Header of table
                    excelWorksheet.Row(1).Height = 20;
                    excelWorksheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    excelWorksheet.Row(1).Style.Font.Bold = true;

                    excelWorksheet.Cells[1, 1].Value = "UniqueNo";
                    excelWorksheet.Cells[1, 2].Value = "ReferralParty";
                    excelWorksheet.Cells[1, 3].Value = "Address";
                    excelWorksheet.Cells[1, 4].Value = "StateName";
                    excelWorksheet.Cells[1, 5].Value = "RegionName";
                    excelWorksheet.Cells[1, 6].Value = "DistrictName";
                    excelWorksheet.Cells[1, 7].Value = "CityName";
                    excelWorksheet.Cells[1, 8].Value = "AreaName";
                    excelWorksheet.Cells[1, 9].Value = "Pincode";
                    excelWorksheet.Cells[1, 10].Value = "Phone";
                    excelWorksheet.Cells[1, 11].Value = "Mobile";
                    excelWorksheet.Cells[1, 12].Value = "GstNo";
                    excelWorksheet.Cells[1, 13].Value = "PanNo";
                    excelWorksheet.Cells[1, 14].Value = "IsActive";


                    recordIndex = 14;

                    foreach (ReferralDetailsResponse record in lstReferralToImport)
                    {
                        excelWorksheet.Cells[recordIndex, 1].Value = record.UniqueNo;
                        excelWorksheet.Cells[recordIndex, 2].Value = record.ReferralParty;
                        excelWorksheet.Cells[recordIndex, 3].Value = record.Address;
                        excelWorksheet.Cells[recordIndex, 4].Value = record.StateName;
                        excelWorksheet.Cells[recordIndex, 5].Value = record.RegionName;
                        excelWorksheet.Cells[recordIndex, 6].Value = record.DistrictName;
                        excelWorksheet.Cells[recordIndex, 7].Value = record.CityName;
                        excelWorksheet.Cells[recordIndex, 8].Value = record.AreaName;
                        excelWorksheet.Cells[recordIndex, 9].Value = record.Pincode;
                        excelWorksheet.Cells[recordIndex, 10].Value = record.Phone;
                        excelWorksheet.Cells[recordIndex, 11].Value = record.Mobile;
                        excelWorksheet.Cells[recordIndex, 12].Value = record.GstNo;
                        excelWorksheet.Cells[recordIndex, 13].Value = record.PanNo;
                        excelWorksheet.Cells[recordIndex, 14].Value = record.IsActive;
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
