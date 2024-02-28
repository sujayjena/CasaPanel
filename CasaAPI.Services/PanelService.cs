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
    public class PanelService :IPanelService
    {
        private IPanelRepository _panelRepository;
        private IFileManager _fileManager;
        public PanelService(IPanelRepository panelRepository, IFileManager fileManager)
        {
            _panelRepository = panelRepository;
            _fileManager = fileManager;
        }
        #region PanelDisplay
        public async Task<int> SavePanelDisplay(PanelDisplaySaveParameters request)
        {
            return await _panelRepository.SavePanelDisplay(request);
        }
        public async Task<IEnumerable<PanelDisplayDetailsResponse>> GetPanelDisplayList(PanelDisplaySearchParameters request)
        {
            return await _panelRepository.GetPanelDisplayList(request);
        }

        public async Task<PanelDisplayDetailsResponse?> GetPanelDisplayDetailsById(long id)
        {
            return await _panelRepository.GetPanelDisplayDetailsById(id);
        }
        #endregion

        #region PanelInventoryIn
        public async Task<int> SavePanelInventoryIn(PanelInventoryInSaveParameters request)
        {
            return await _panelRepository.SavePanelInventoryIn(request);
        }
        public async Task<IEnumerable<PanelInventoryInDetailsResponse>> GetPanelInventoryInList(PanelInventoryInSearchParameters request)
        {
            return await _panelRepository.GetPanelInventoryInList(request);
        }

        public async Task<PanelInventoryInDetailsResponse?> GetPanelInventoryInDetailsById(long id)
        {
            return await _panelRepository.GetPanelInventoryInDetailsById(id);
        }
        #endregion

        #region PanelInventoryOut
        public async Task<int> SavePanelInventoryOut(PanelInventoryOutSaveParameters request)
        {
            return await _panelRepository.SavePanelInventoryOut(request);
        }
        public async Task<IEnumerable<PanelInventoryOutDetailsResponse>> GetPanelInventoryOutList(PanelInventoryOutSearchParameters request)
        {
            return await _panelRepository.GetPanelInventoryOutList(request);
        }

        public async Task<PanelInventoryOutDetailsResponse?> GetPanelInventoryOutDetailsById(long id)
        {
            return await _panelRepository.GetPanelInventoryOutDetailsById(id);
        }
        #endregion
    }
}
