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
    public class VendorsService : IVendorsService
    {
        private IVendorsRepository _vendorsRepository;
        private IFileManager _fileManager;
        public VendorsService(IVendorsRepository vendorsRepository, IFileManager fileManager)
        {
            _vendorsRepository = vendorsRepository;
            _fileManager = fileManager;
        }
        #region Vender
        public async Task<int> SaveVender(VenderSaveParameters request)
        {
            return await _vendorsRepository.SaveVender(request);
        }
        public async Task<IEnumerable<VenderDetailsResponse>> GetVenderList(VenderSearchParameters request)
        {
            return await _vendorsRepository.GetVenderList(request);
        }

        public async Task<VenderDetailsResponse?> GetVenderDetailsById(long id)
        {
            return await _vendorsRepository.GetVenderDetailsById(id);
        }

        public async Task<int> UpdateVenderStatus(VenderUpdateStatus request)
        {
            return await _vendorsRepository.UpdateVenderStatus(request);
        }
        #endregion
    }
}
