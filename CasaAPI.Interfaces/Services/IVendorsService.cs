using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Services
{
    public interface IVendorsService
    {
        #region Vender
        Task<int> SaveVender(VenderSaveParameters request);
        Task<IEnumerable<VenderDetailsResponse>> GetVenderList(VenderSearchParameters request);
        Task<VenderDetailsResponse?> GetVenderDetailsById(long id);
        Task<int> UpdateVenderStatus(VenderUpdateStatus request);
        #endregion
    }
}
