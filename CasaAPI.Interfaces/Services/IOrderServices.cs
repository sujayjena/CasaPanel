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
    }
}
