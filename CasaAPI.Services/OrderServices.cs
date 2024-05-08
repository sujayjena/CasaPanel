using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Services
{
    public class OrderServices: IOrderServices
    {
        private IOrderRepository _orderRepository;
        private IFileManager _fileManager;
        public OrderServices(IOrderRepository orderRepository, IFileManager fileManager)
        {
            _orderRepository = orderRepository;
            _fileManager = fileManager;
        }

        #region DispatchOrder
        public async Task<int> SaveDispatchOrder(DispatchOrderSaveParameters request)
        {
            return await _orderRepository.SaveDispatchOrder(request);
        }
        public async Task<IEnumerable<DispatchOrderDetailsResponse>> GetDispatchOrderList(DispatchOrderSearchParameters request)
        {
            return await _orderRepository.GetDispatchOrderList(request);
        }
        public async Task<IEnumerable<DispatchPanelDisplayDetailsResponse>> GetDispatchPanelDisplayOrderList(long dispatchOrderId)
        {
            return await _orderRepository.GetDispatchPanelDisplayOrderList(dispatchOrderId);
        }
        public async Task<DispatchOrderDetailsResponse?> GetDispatchOrderDetailsById(long id)
        {
            return await _orderRepository.GetDispatchOrderDetailsById(id);
        }
        #endregion

        #region Orders
        public async Task<int> SaveOrder(OrderSaveParameters request)
        {
            return await _orderRepository.SaveOrder(request);
        }
        public async Task<IEnumerable<OrderListResponse>> GetOrderList(OrderSearchParameters request)
        {
            return await _orderRepository.GetOrderList(request);
        }
        public async Task<OrderListResponse?> GetOrderById(int id)
        {
            return await _orderRepository.GetOrderById(id);
        }
        public async Task<int> SaveOrderDetails(OrderDetailsSaveParameters request)
        {
            return await _orderRepository.SaveOrderDetails(request);
        }
        public async Task<IEnumerable<OrderDetailsResponse>> GetOrderDetailsList(OrderDetailsSearchParameters request)
        {
            return await _orderRepository.GetOrderDetailsList(request);
        }
        #endregion
    }
}
