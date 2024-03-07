using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using CasaAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CasaAPI.Controllers
{
    public class ManageServicesController :  CustomBaseController
    {
        private ResponseModel _response;
        private ICuttingPlanService _cuttingPlanService;
        private IFileManager _fileManager;

        public ManageServicesController(ICuttingPlanService cuttingPlanService, IFileManager fileManager)
        {
            _cuttingPlanService = cuttingPlanService;
             _fileManager = fileManager;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        #region ManageCuttingPlan
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetCuttingPlanList(SearchCuttingPlanRequest request)
        {
            IEnumerable<CuttingPlanResponse> lstCuttingPlan = await _cuttingPlanService.GetCuttingPlanList(request);
            _response.Data = lstCuttingPlan.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveCuttingPlan(CuttingPlanSaveParameters cuttingPlanRequest)
        {
            int result = await _cuttingPlanService.SaveCuttingPlan(cuttingPlanRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Cutting Plan is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Cutting Plan details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetCuttingPlanDetails(long id)
        {
            CuttingPlanResponse? cuttingPlan;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                cuttingPlan = await _cuttingPlanService.GetCuttingPlanDetailsById(id);
                _response.Data = cuttingPlan;
            }

            return _response;
        }
        #endregion

        #region QuoteTilesCutting
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetQuoteTilesCuttingList(SearchQuoteTilesCuttingRequest request)
        {
            IEnumerable<QuoteTilesCuttingResponse> lstQuoteTilesCutting = await _cuttingPlanService.GetQuoteTilesCuttingList(request);
            _response.Data = lstQuoteTilesCutting.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveQuoteTilesCutting(QuoteTilesCuttingSaveParameters cuttingPlanRequest)
        {
            int result = await _cuttingPlanService.SaveQuoteTilesCutting(cuttingPlanRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Quote Tiles Cutting is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "QuoteTiles Cutting details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetQuoteTilesCuttingDetails(long id)
        {
            QuoteTilesCuttingResponse? quoteTilesCutting;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                quoteTilesCutting = await _cuttingPlanService.GetQuoteTilesCuttingDetails(id);
                _response.Data = quoteTilesCutting;
            }

            return _response;
        }
        #endregion

        #region PanelPlanning
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPanelPlanningList(SearchPanelPlanningRequest request)
        {
            IEnumerable<PanelPlanningResponse> lstPanelPlanning = await _cuttingPlanService.GetPanelPlanningList(request);
            _response.Data = lstPanelPlanning.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePanelPlanning([FromForm] PanelPlanningSaveParameters panelPlanningRequest)
        {
            if (panelPlanningRequest.ImageUploadFile?.Length > 0)
            {
                panelPlanningRequest.ImageUpload = _fileManager.UploadProfilePicture(panelPlanningRequest.ImageUploadFile);
            }
            int result = await _cuttingPlanService.SavePanelPlanning(panelPlanningRequest);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Panel Planning is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Panel Planning details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPanelPlanningDetails(long id)
        {
            PanelPlanningResponse? panelPlanning;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                panelPlanning = await _cuttingPlanService.GetPanelPlanningDetailsById(id);
                _response.Data = panelPlanning;
            }

            return _response;
        }
        #endregion

        #region QuotePanelDesign
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetQuotePanelDesignList(SearchQuotePanelDesignRequest request)
        {
            IEnumerable<QuotePanelDesignResponse> lstQuotePanelDesign = await _cuttingPlanService.GetQuotePanelDesignList(request);
            _response.Data = lstQuotePanelDesign.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveQuotePanelDesign(QuotePanelDesignSaveParameters Request)
        {
            int result = await _cuttingPlanService.SaveQuotePanelDesign(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Quote Panel Design is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = " Quote Panel Design details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetQuotePanelDesignDetails(long id)
        {
            QuotePanelDesignResponse? quotePanelDesign;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                quotePanelDesign = await _cuttingPlanService.GetQuotePanelDesignDetailsById(id);
                _response.Data = quotePanelDesign;
            }

            return _response;
        }
        #endregion

        #region BinderPlanning
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBinderPlanningList(SearchBinderPlanningRequest request)
        {
            IEnumerable<BinderPlanningResponse> lstBinderPlanning = await _cuttingPlanService.GetBinderPlanningList(request);
            _response.Data = lstBinderPlanning.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBinderPlanning(BinderPlanningSaveParameters Request)
        {
            int result = await _cuttingPlanService.SaveBinderPlanning(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Binder Planning is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Binder Planning details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetBinderPlanningDetails(long id)
        {
            BinderPlanningResponse? binderPlanning;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                binderPlanning = await _cuttingPlanService.GetBinderPlanningDetailsById(id);
                _response.Data = binderPlanning;
            }

            return _response;
        }
        #endregion

        #region BinderQuote
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetBinderQuoteList(SearchBinderQuoteRequest request)
        {
            IEnumerable<BinderQuoteResponse> lstBinderQuote = await _cuttingPlanService.GetBinderQuoteList(request);
            _response.Data = lstBinderQuote.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveBinderQuote(BinderQuoteSaveParameters Request)
        {
            int result = await _cuttingPlanService.SaveBinderQuote(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Binder Quote is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Binder Quote details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetBinderQuoteDetails(long id)
        {
            BinderQuoteResponse? binderPlanning;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                binderPlanning = await _cuttingPlanService.GetBinderQuoteDetailsById(id);
                _response.Data = binderPlanning;
            }

            return _response;
        }
        #endregion

        #region PrintingPlan
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPrintingPlanList(SearchPrintingPlanRequest request)
        {
            IEnumerable<PrintingPlanResponse> lstPrintingPlan = await _cuttingPlanService.GetPrintingPlanList(request);
            _response.Data = lstPrintingPlan.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePrintingPlan(PrintingPlanSaveParameters Request)
        {
            int result = await _cuttingPlanService.SavePrintingPlan(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Printing Plan is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Printing Plan details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPrintingPlanDetails(long id)
        {
            PrintingPlanResponse? printingPlan;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                printingPlan = await _cuttingPlanService.GetPrintingPlanDetailsById(id);
                _response.Data = printingPlan;
            }

            return _response;
        }
        #endregion

        #region PrintingQuote
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPrintingQuoteList(SearchPrintingQuoteRequest request)
        {
            IEnumerable<PrintingQuoteResponse> lstPrintingQuote = await _cuttingPlanService.GetPrintingQuoteList(request);
            _response.Data = lstPrintingQuote.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePrintingQuote(PrintingQuoteSaveParameters Request)
        {
            int result = await _cuttingPlanService.SavePrintingQuote(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Printing Quote is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Printing Quote details saved sucessfully";
            }
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPrintingQuoteDetails(long id)
        {
            PrintingQuoteResponse? printingQuote;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                printingQuote = await _cuttingPlanService.GetPrintingQuoteDetailsById(id);
                _response.Data = printingQuote;
            }

            return _response;
        }
        #endregion
    }
}
