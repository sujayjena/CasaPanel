using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IPanelRepository
    {
        #region PanelDisplay
        Task<int> SavePanelDisplay(PanelDisplaySaveParameters parameters);
        Task<IEnumerable<PanelDisplayDetailsResponse>> GetPanelDisplayList(PanelDisplaySearchParameters parameters);
        Task<PanelDisplayDetailsResponse?> GetPanelDisplayDetailsById(long id);
        #endregion
        #region PanelInventoryIn
        Task<int> SavePanelInventoryIn(PanelInventoryInSaveParameters parameters);
        Task<IEnumerable<PanelInventoryInDetailsResponse>> GetPanelInventoryInList(PanelInventoryInSearchParameters parameters);
        Task<PanelInventoryInDetailsResponse?> GetPanelInventoryInDetailsById(long id);
        #endregion

        #region PanelInventoryOut
        Task<int> SavePanelInventoryOut(PanelInventoryOutSaveParameters parameters);
        Task<IEnumerable<PanelInventoryOutDetailsResponse>> GetPanelInventoryOutList(PanelInventoryOutSearchParameters parameters);
        Task<PanelInventoryOutDetailsResponse?> GetPanelInventoryOutDetailsById(long id);
        #endregion
    }
}
