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

        #region Order

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveOrder(OrderSaveParameters request)
        {
            int result = await _orderService.SaveOrder(request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Order is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Order details saved sucessfully";
            }

            if (result > 0)
            {
                // Save/Update Order Details
                foreach (var item in request.orderDetailsList)
                {
                    var vOrderDetailsSaveParameters = new OrderDetailsSaveParameters()
                    {
                        Id = item.Id,
                        OrderId = result,
                        CollectionId = item.CollectionId,
                        BaseDesignId = item.BaseDesignId,
                        SizeId = item.SizeId,
                        SurfaceId = item.SurfaceId,
                        ThicknessId = item.ThicknessId,
                        Quantity = item.Quantity,
                    };

                    int result_OrderDetails = await _orderService.SaveOrderDetails(vOrderDetailsSaveParameters);
                }
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetOrderList(OrderSearchParameters request)
        {
            IEnumerable<OrderListResponse> lstDealer = await _orderService.GetOrderList(request);
            _response.Data = lstDealer.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetOrderById(int id)
        {
            var vOrderDetailsByIdResponse = new OrderDetailsByIdResponse();

            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                var vResultObj = await _orderService.GetOrderById(id);
                if (vResultObj != null)
                {
                    vOrderDetailsByIdResponse.Id = vResultObj.Id;
                    vOrderDetailsByIdResponse.OrderNo = vResultObj.OrderNo;
                    vOrderDetailsByIdResponse.OrderDate = vResultObj.OrderDate;
                    vOrderDetailsByIdResponse.EmployeeId = vResultObj.EmployeeId;
                    vOrderDetailsByIdResponse.EmployeeName = vResultObj.EmployeeName;
                    vOrderDetailsByIdResponse.CustomerId = vResultObj.CustomerId;
                    vOrderDetailsByIdResponse.CompanyName = vResultObj.CompanyName;
                    vOrderDetailsByIdResponse.MobileNo = vResultObj.MobileNo;
                    vOrderDetailsByIdResponse.CustomerTypeId = vResultObj.CustomerTypeId;
                    vOrderDetailsByIdResponse.CustomerTypeName = vResultObj.CustomerTypeName;
                    vOrderDetailsByIdResponse.EmailId = vResultObj.EmailId;
                    vOrderDetailsByIdResponse.RefName = vResultObj.RefName;
                    vOrderDetailsByIdResponse.StateId = vResultObj.StateId;
                    vOrderDetailsByIdResponse.StateName = vResultObj.StateName;
                    vOrderDetailsByIdResponse.RegionId = vResultObj.RegionId;
                    vOrderDetailsByIdResponse.RegionName = vResultObj.RegionName;
                    vOrderDetailsByIdResponse.DistrictId = vResultObj.DistrictId;
                    vOrderDetailsByIdResponse.DistrictName = vResultObj.DistrictName;
                    vOrderDetailsByIdResponse.AreaId = vResultObj.AreaId;
                    vOrderDetailsByIdResponse.AreaName = vResultObj.AreaName;
                    vOrderDetailsByIdResponse.CityId = vResultObj.CityId;
                    vOrderDetailsByIdResponse.CityName = vResultObj.CityName;
                    vOrderDetailsByIdResponse.Pincode = vResultObj.Pincode;
                    vOrderDetailsByIdResponse.BrandId = vResultObj.BrandId;
                    vOrderDetailsByIdResponse.BrandName = vResultObj.BrandName;
                    vOrderDetailsByIdResponse.PanelQty = vResultObj.PanelQty;
                    vOrderDetailsByIdResponse.BinderQty = vResultObj.BinderQty;
                    vOrderDetailsByIdResponse.Remarks = vResultObj.Remarks;
                    vOrderDetailsByIdResponse.GrandTotalQty = vResultObj.GrandTotalQty;
                    vOrderDetailsByIdResponse.StatusId = vResultObj.StatusId;
                    vOrderDetailsByIdResponse.StatusName = vResultObj.StatusName;
                    vOrderDetailsByIdResponse.IsActive = vResultObj.IsActive;
                    vOrderDetailsByIdResponse.CreatorName = vResultObj.CreatorName;
                    vOrderDetailsByIdResponse.CreatedBy = vResultObj.CreatedBy;

                    // Order Details
                    var vSearchObj = new OrderDetailsSearchParameters()
                    {
                        OrderId = vResultObj.Id,
                        pagination = new PaginationParameters()
                    };

                    var objOrderDetailsList = await _orderService.GetOrderDetailsList(vSearchObj);
                    foreach (var item in objOrderDetailsList)
                    {
                        vOrderDetailsByIdResponse.orderDetails.Add(item);
                    }
                }

                _response.Data = vOrderDetailsByIdResponse;
            }

            return _response;
        }

        #endregion

        #region Order Booking
        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> SaveOrderBooking(OrderBooking_Request request)
        {
            //Image Upload
            if (!string.IsNullOrWhiteSpace(request.ImageFile_Base64))
            {
                var vUploadFile = _fileManager.UploadDocumentsBase64ToFile(request.ImageFile_Base64, "\\Uploads\\Order\\", request.ImageOriginalFileName);

                if (!string.IsNullOrWhiteSpace(vUploadFile))
                {
                    request.ImageFileName = vUploadFile;
                }
            }

            int result = await _orderService.SaveOrderBooking(request);
            _response.IsSuccess = false;

            if (result == (int)SaveEnums.NoRecordExists)
            {
                _response.Message = "No record exists";
            }
            else if (result == (int)SaveEnums.NameExists)
            {
                _response.Message = "Order Booking is already exists";
            }
            else if (result == (int)SaveEnums.NoResult)
            {
                _response.Message = "Something went wrong, please try again";
            }
            else
            {
                _response.IsSuccess = true;
                _response.Message = "Order Booking details saved sucessfully";
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetOrderBookingList(OrderBooking_Search request)
        {
            IEnumerable<OrderBooking_Response> lstDealer = await _orderService.GetOrderBookingList(request);
            _response.Data = lstDealer.ToList();
            _response.Total = request.pagination.Total;
            return _response;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ResponseModel> GetOrderBookingById(int id)
        {
            if (id <= 0)
            {
                _response.IsSuccess = false;
                _response.Message = ValidationConstants.Id_Required_Msg;
            }
            else
            {
                var vResultObj = await _orderService.GetOrderBookingById(id);
                _response.Data = vResultObj;
            }

            return _response;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ResponseModel> GetOrderBooking_Collection_BaseDesign_Size_Surface_List_ById(OrderBooking_Collection_BaseDesign_Size_Surface_Search parameters)
        {
            var vResultObj = await _orderService.GetOrderBooking_Collection_BaseDesign_Size_Surface_List_ById(parameters);
            _response.Data = vResultObj;

            return _response;
        }
        #endregion
    }
}
