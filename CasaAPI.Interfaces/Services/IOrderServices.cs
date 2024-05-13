using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Services
{
    public interface IOrderServices
    {
        #region DispatchOrder
        Task<int> SaveDispatchOrder(DispatchOrderSaveParameters request);
        Task<IEnumerable<DispatchOrderDetailsResponse>> GetDispatchOrderList(DispatchOrderSearchParameters request);
        Task<DispatchOrderDetailsResponse?> GetDispatchOrderDetailsById(long id);
        Task<IEnumerable<DispatchPanelDisplayDetailsResponse>> GetDispatchPanelDisplayOrderList(long dispatchOrderId);
        #endregion

        #region Order
        Task<int> SaveOrder(OrderSaveParameters request);
        Task<IEnumerable<OrderListResponse>> GetOrderList(OrderSearchParameters request);
        Task<OrderListResponse?> GetOrderById(int id);
        Task<int> SaveOrderDetails(OrderDetailsSaveParameters request);
        Task<IEnumerable<OrderDetailsResponse>> GetOrderDetailsList(OrderDetailsSearchParameters parameters);
        #endregion

        #region Order Booking
        Task<int> SaveOrderBooking(OrderBooking_Request request);
        Task<IEnumerable<OrderBooking_Response>> GetOrderBookingList(OrderBooking_Search request);
        Task<OrderBooking_Response?> GetOrderBookingById(int id);
        Task<IEnumerable<OrderBooking_Collection_BaseDesign_Size_Surface_Response>> GetOrderBooking_Collection_BaseDesign_Size_Surface_List_ById(OrderBooking_Collection_BaseDesign_Size_Surface_Search parameters);
        #endregion
    }
}
