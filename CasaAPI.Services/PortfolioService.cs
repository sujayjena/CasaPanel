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
    public class PortfolioService : IPortfolioService
    {
        private IPortfolioRepository _portfolioRepository;
        private IFileManager _fileManager;
        public PortfolioService(IPortfolioRepository portfolioRepository, IFileManager fileManager)
        {
            _portfolioRepository = portfolioRepository;
            _fileManager = fileManager;
        }
        #region PanelDisplay
        public async Task<int> SavePortfolio(PortfolioSaveParameters request)
        {
            return await _portfolioRepository.SavePortfolio(request);
        }
        public async Task<IEnumerable<PortfolioDetailsResponse>> GetPortfolioList(PortfolioSearchParameters request)
        {
            return await _portfolioRepository.GetPortfolioList(request);
        }

        public async Task<PortfolioDetailsResponse?> GetPortfolioDetailsById(long id)
        {
            return await _portfolioRepository.GetPortfolioDetailsById(id);
        }
        #endregion
    }
}
