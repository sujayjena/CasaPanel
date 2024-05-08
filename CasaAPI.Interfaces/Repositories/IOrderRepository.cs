﻿using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        #region DispatchOrder
        Task<int> SaveDispatchOrder(DispatchOrderSaveParameters parameters);
        Task<IEnumerable<DispatchOrderDetailsResponse>> GetDispatchOrderList(DispatchOrderSearchParameters parameters);
        Task<DispatchOrderDetailsResponse?> GetDispatchOrderDetailsById(long id);
        Task<IEnumerable<DispatchPanelDisplayDetailsResponse>> GetDispatchPanelDisplayOrderList(long dispatchOrderId);
        #endregion

        #region Order
        Task<int> SaveOrder(OrderSaveParameters parameters);
        Task<IEnumerable<OrderListResponse>> GetOrderList(OrderSearchParameters parameters);
        Task<OrderListResponse?> GetOrderById(int id);
        Task<int> SaveOrderDetails(OrderDetailsSaveParameters parameters);
        Task<IEnumerable<OrderDetailsResponse>> GetOrderDetailsList(OrderDetailsSearchParameters parameters);
        #endregion
    }
}
