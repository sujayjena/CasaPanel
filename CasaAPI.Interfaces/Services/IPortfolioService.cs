using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Services
{
    public interface IPortfolioService
    {
        #region PanelDisplay
        Task<int> SavePortfolio(PortfolioSaveParameters request);
        Task<IEnumerable<PortfolioDetailsResponse>> GetPortfolioList(PortfolioSearchParameters request);
        Task<PortfolioDetailsResponse?> GetPortfolioDetailsById(long id);
        #endregion
    }
}
