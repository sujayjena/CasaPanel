using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models.Enums;
using CasaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CasaAPI.Models.Constants;
using CasaAPI.Services;

namespace CasaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PanelController : ControllerBase
    {
        private ResponseModel _response;
        private IPanelService _panelService;
        private IFileManager _fileManager;
        public PanelController(IPanelService panelService, IFileManager fileManager)
        {
            _panelService = panelService;
            _fileManager = fileManager;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
        #region PanelDisplay
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePanelDisplay(PanelDisplaySaveParameters Request)
        {
            int result = await _panelService.SavePanelDisplay(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Panel Display is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "PanelDisplay details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPanelDisplayList(PanelDisplaySearchParameters request)
        {
            IEnumerable<PanelDisplayDetailsResponse> lstDealer = await _panelService.GetPanelDisplayList(request);
            _response.Data = lstDealer.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPanelDisplayDetails(long id)
        {
            PanelDisplayDetailsResponse? panelDisplay;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                panelDisplay = await _panelService.GetPanelDisplayDetailsById(id);
                _response.Data = panelDisplay;
            }

            return _response;
        }
        #endregion

        #region PanelInventoryIn
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePanelInventoryIn(PanelInventoryInSaveParameters Request)
        {
            int result = await _panelService.SavePanelInventoryIn(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Panel InventoryIn  is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "PanelInventoryIn details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPanelInventoryInList(PanelInventoryInSearchParameters request)
        {
            IEnumerable<PanelInventoryInDetailsResponse> lstDealer = await _panelService.GetPanelInventoryInList(request);
            _response.Data = lstDealer.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPanelInventoryInDetails(long id)
        {
            PanelInventoryInDetailsResponse? panelInventoryIn;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                panelInventoryIn = await _panelService.GetPanelInventoryInDetailsById(id);
                _response.Data = panelInventoryIn;
            }

            return _response;
        }
        #endregion

        #region PanelInventoryOut
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SavePanelInventoryOut(PanelInventoryOutSaveParameters Request)
        {
            int result = await _panelService.SavePanelInventoryOut(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Panel InventoryOut  is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "PanelInventoryOut details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetPanelInventoryOutList(PanelInventoryOutSearchParameters request)
        {
            IEnumerable<PanelInventoryOutDetailsResponse> lstDealer = await _panelService.GetPanelInventoryOutList(request);
            _response.Data = lstDealer.ToList();
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetPanelInventoryOutDetails(long id)
        {
            PanelInventoryOutDetailsResponse? panelInventoryOut;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                panelInventoryOut = await _panelService.GetPanelInventoryOutDetailsById(id);
                _response.Data = panelInventoryOut;
            }

            return _response;
        }
        #endregion
    }
}
