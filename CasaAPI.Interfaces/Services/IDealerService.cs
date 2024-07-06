using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Services
{
    public interface IDealerService
    {
        #region Dealer
        Task<int> SaveDealer(DealerSaveParameters request);
        Task<IEnumerable<DealerDetailsResponse>> GetDealerList(DealerSearchParameters request);
        Task<DealerDetailsResponse?> GetDealerDetailsById(long id);
        Task<int> UpdateDealerStatus(DealerStatusUpdate request);
        #endregion

        #region DealerAddress
        Task<int> SaveDealerAddress(DealerAddressSaveParameters request);
        Task<IEnumerable<DealerAddressDetailsResponse>> GetDealerAddressList(DealerAddressSearchParameters request);
        Task<DealerAddressDetailsResponse?> GetDealerAddressDetailsById(long id);
        #endregion

        #region DealerContactDetails
        Task<int> SaveDealerContactDetails(DealerContactDetailsSaveParameters request);
        Task<IEnumerable<DealerContactDetailsResponse>> GetDealerContactDetailsList(DealerContactDetailsSearchParameters request);
        Task<DealerContactDetailsResponse?> GetDealerContactDetailsById(long id);
        #endregion
    }
}
