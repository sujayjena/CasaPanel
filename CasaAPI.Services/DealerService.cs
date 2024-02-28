using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using CasaAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Services
{
    public class DealerService : IDealerService
    {
        private IDealerRepository _dealerRepository;
        private IFileManager _fileManager;
        public DealerService(IDealerRepository dealerRepository, IFileManager fileManager)
        {
            _dealerRepository = dealerRepository;
            _fileManager = fileManager;
        }
        #region Dealer
        public async Task<int> SaveDealer(DealerSaveParameters request)
        {
            return await _dealerRepository.SaveDealer(request);
        }
        public async Task<IEnumerable<DealerDetailsResponse>> GetDealerList(DealerSearchParameters request)
        {
            return await _dealerRepository.GetDealerList(request);
        }

        public async Task<DealerDetailsResponse?> GetDealerDetailsById(long id)
        {
            return await _dealerRepository.GetDealerDetailsById(id);
        }
        #endregion

        #region DealerAddress
        public async Task<int> SaveDealerAddress(DealerAddressSaveParameters request)
        {
            return await _dealerRepository.SaveDealeAddress(request);
        }
        public async Task<IEnumerable<DealerAddressDetailsResponse>> GetDealerAddressList(DealerAddressSearchParameters request)
        {
            return await _dealerRepository.GetDealerAddressList(request);
        }

        public async Task<DealerAddressDetailsResponse?> GetDealerAddressDetailsById(long id)
        {
            return await _dealerRepository.GetDealerAddressDetailsById(id);
        }
        #endregion

        #region DealerContactDetails
        public async Task<int> SaveDealerContactDetails(DealerContactDetailsSaveParameters request)
        {
            return await _dealerRepository.SaveDealerContactDetails(request);
        }
        public async Task<IEnumerable<DealerContactDetailsResponse>> GetDealerContactDetailsList(DealerContactDetailsSearchParameters request)
        {
            return await _dealerRepository.GetDealerContactDetailsList(request);
        }

        public async Task<DealerContactDetailsResponse?> GetDealerContactDetailsById(long id)
        {
            return await _dealerRepository.GetDealerContactDetailsById(id);
        }
        #endregion
    }
}
