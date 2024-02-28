using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Services
{
    public interface IPanelService
    {
        #region PanelDisplay
         Task<int> SavePanelDisplay(PanelDisplaySaveParameters request);
        Task<IEnumerable<PanelDisplayDetailsResponse>> GetPanelDisplayList(PanelDisplaySearchParameters request);
        Task<PanelDisplayDetailsResponse?> GetPanelDisplayDetailsById(long id);
        #endregion
        #region PanelInventoryIn
        Task<int> SavePanelInventoryIn(PanelInventoryInSaveParameters request);
        Task<IEnumerable<PanelInventoryInDetailsResponse>> GetPanelInventoryInList(PanelInventoryInSearchParameters request);
        Task<PanelInventoryInDetailsResponse?> GetPanelInventoryInDetailsById(long id);
        #endregion

        #region PanelInventoryOut
        Task<int> SavePanelInventoryOut(PanelInventoryOutSaveParameters request);
        Task<IEnumerable<PanelInventoryOutDetailsResponse>> GetPanelInventoryOutList(PanelInventoryOutSearchParameters request);
        Task<PanelInventoryOutDetailsResponse?> GetPanelInventoryOutDetailsById(long id);
        #endregion
    }
}
