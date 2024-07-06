using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IDealerRepository
    {

        #region Dealer
        Task<int> SaveDealer(DealerSaveParameters parameters);
        Task<IEnumerable<DealerDetailsResponse>> GetDealerList(DealerSearchParameters parameters);
        Task<DealerDetailsResponse?> GetDealerDetailsById(long id);
        Task<int> UpdateDealerStatus(DealerStatusUpdate request);
        #endregion
        #region DealerAddress
        Task<int> SaveDealeAddress(DealerAddressSaveParameters parameters);
        Task<IEnumerable<DealerAddressDetailsResponse>> GetDealerAddressList(DealerAddressSearchParameters parameters);
        Task<DealerAddressDetailsResponse?> GetDealerAddressDetailsById(long id);
        #endregion
        #region DealerContactDetails
        Task<int> SaveDealerContactDetails(DealerContactDetailsSaveParameters parameters);
        Task<IEnumerable<DealerContactDetailsResponse>> GetDealerContactDetailsList(DealerContactDetailsSearchParameters parameters);
        Task<DealerContactDetailsResponse?> GetDealerContactDetailsById(long id);
        #endregion
    }
}
