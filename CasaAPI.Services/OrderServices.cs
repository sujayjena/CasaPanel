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
    }
}
