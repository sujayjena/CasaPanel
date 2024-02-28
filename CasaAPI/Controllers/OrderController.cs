using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models.Constants;
using CasaAPI.Models.Enums;
using CasaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CasaAPI.Services;

namespace CasaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private ResponseModel _response;
        private IOrderServices _orderService;
        private IFileManager _fileManager;
        public OrderController(IOrderServices orderService, IFileManager fileManager)
        {
            _orderService = orderService;
            _fileManager = fileManager;
            _response = new ResponseModel();
            _response.IsSuccess = true;
        }
        #region DispatchOrder
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveDispatchOrder(DispatchOrderSaveParameters Request)
        {
            int result = await _orderService.SaveDispatchOrder(Request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Dispatch Order is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Dispatch Order details saved sucessfully";
            }
            return _response;
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetDispatchOrderList(DispatchOrderSearchParameters request)
        {
            IEnumerable<DispatchOrderDetailsResponse> lstDealer = await _orderService.GetDispatchOrderList(request);
            _response.Data = lstDealer.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDispatchPanelDisplayOrderList(long dispatchOrderId)
        {
            IEnumerable<DispatchPanelDisplayDetailsResponse> lstDispatchPanel = await _orderService.GetDispatchPanelDisplayOrderList(dispatchOrderId);
            _response.Data = lstDispatchPanel.ToList();
            return _response;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetDispatchOrderDetails(long id)
        {
            DispatchOrderDetailsResponse? dispatchOrder;

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                dispatchOrder = await _orderService.GetDispatchOrderDetailsById(id);
                _response.Data = dispatchOrder;
            }

            return _response;
        }
        #endregion
    }
}
