using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IPortfolioRepository
    {
        #region Portfolio
        Task<int> SavePortfolio(PortfolioSaveParameters parameters);
        Task<IEnumerable<PortfolioDetailsResponse>> GetPortfolioList(PortfolioSearchParameters parameters);
        Task<PortfolioDetailsResponse?> GetPortfolioDetailsById(long id);
        #endregion
    }
}
