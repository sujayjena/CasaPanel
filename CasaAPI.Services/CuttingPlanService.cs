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
    public class CuttingPlanService: ICuttingPlanService
    {
        private ICuttingPlanRepository _cuttingPlanRepository;
        private IFileManager _fileManager;
        public CuttingPlanService(ICuttingPlanRepository cuttingPlanRepository, IFileManager fileManager)
        {
            _cuttingPlanRepository = cuttingPlanRepository;
            _fileManager = fileManager;
        }
        #region Cutting
        public async Task<int> SaveCuttingPlan(CuttingPlanSaveParameters request)
        {
            return await _cuttingPlanRepository.SaveCuttingPlan(request);
        }
        public async Task<IEnumerable<CuttingPlanResponse>> GetCuttingPlanList(SearchCuttingPlanRequest request)
        {
            return await _cuttingPlanRepository.GetCuttingPlanList(request);
        }
        public async Task<CuttingPlanResponse?> GetCuttingPlanDetailsById(long id)
        {
            return await _cuttingPlanRepository.GetCuttingPlanDetailsById(id);
        }
        #endregion
        #region QuoteTilesCutting
        public async Task<int> SaveQuoteTilesCutting(QuoteTilesCuttingSaveParameters request)
        {
            return await _cuttingPlanRepository.SaveQuoteTilesCutting(request);
        }
        public async Task<IEnumerable<QuoteTilesCuttingResponse>> GetQuoteTilesCuttingList(SearchQuoteTilesCuttingRequest request)
        {
            return await _cuttingPlanRepository.GetQuoteTilesCuttingList(request);
        }
        public async Task<QuoteTilesCuttingResponse?> GetQuoteTilesCuttingDetails(long id)
        {
            return await _cuttingPlanRepository.GetQuoteTilesCuttingDetailsById(id);
        }
        #endregion

        #region PanelPlanning
        public async Task<int> SavePanelPlanning(PanelPlanningSaveParameters request)
        {
            return await _cuttingPlanRepository.SavePanelPlanning(request);
        }
        public async Task<IEnumerable<PanelPlanningResponse>> GetPanelPlanningList(SearchPanelPlanningRequest request)
        {
            return await _cuttingPlanRepository.GetPanelPlanningList(request);
        }
        public async Task<PanelPlanningResponse?> GetPanelPlanningDetailsById(long id)
        {
            return await _cuttingPlanRepository.GetPanelPlanningDetailsById(id);
        }
        #endregion
        #region QuotePanelDesign
        public async Task<int> SaveQuotePanelDesign(QuotePanelDesignSaveParameters request)
        {
            return await _cuttingPlanRepository.SaveQuotePanelDesign(request);
        }
        public async Task<IEnumerable<QuotePanelDesignResponse>> GetQuotePanelDesignList(SearchQuotePanelDesignRequest request)
        {
            return await _cuttingPlanRepository.GetQuotePanelDesignList(request);
        }
        public async Task<QuotePanelDesignResponse?> GetQuotePanelDesignDetailsById(long id)
        {
            return await _cuttingPlanRepository.GetQuotePanelDesignDetailsById(id);
        }
        #endregion

        #region BinderPlanning
        public async Task<int> SaveBinderPlanning(BinderPlanningSaveParameters request)
        {
            return await _cuttingPlanRepository.SaveBinderPlanning(request);
        }
        public async Task<IEnumerable<BinderPlanningResponse>> GetBinderPlanningList(SearchBinderPlanningRequest request)
        {
            return await _cuttingPlanRepository.GetBinderPlanningList(request);
        }
        public async Task<BinderPlanningResponse?> GetBinderPlanningDetailsById(long id)
        {
            return await _cuttingPlanRepository.GetBinderPlanningDetailsById(id);
        }
        #endregion

        #region BinderQuote
        public async Task<int> SaveBinderQuote(BinderQuoteSaveParameters request)
        {
            return await _cuttingPlanRepository.SaveBinderQuote(request);
        }
        public async Task<IEnumerable<BinderQuoteResponse>> GetBinderQuoteList(SearchBinderQuoteRequest request)
        {
            return await _cuttingPlanRepository.GetBinderQuoteList(request);
        }
        public async Task<BinderQuoteResponse?> GetBinderQuoteDetailsById(long id)
        {
            return await _cuttingPlanRepository.GetBinderQuoteDetailsById(id);
        }
        #endregion

        #region PrintingPlan
        public async Task<int> SavePrintingPlan(PrintingPlanSaveParameters request)
        {
            return await _cuttingPlanRepository.SavePrintingPlan(request);
        }
        public async Task<IEnumerable<PrintingPlanResponse>> GetPrintingPlanList(SearchPrintingPlanRequest request)
        {
            return await _cuttingPlanRepository.GetPrintingPlanList(request);
        }
        public async Task<PrintingPlanResponse?> GetPrintingPlanDetailsById(long id)
        {
            return await _cuttingPlanRepository.GetPrintingPlanDetailsById(id);
        }
        #endregion

        #region PrintingQuote
        public async Task<int> SavePrintingQuote(PrintingQuoteSaveParameters request)
        {
            return await _cuttingPlanRepository.SavePrintingQuote(request);
        }
        public async Task<IEnumerable<PrintingQuoteResponse>> GetPrintingQuoteList(SearchPrintingQuoteRequest request)
        {
            return await _cuttingPlanRepository.GetPrintingQuoteList(request);
        }
        public async Task<PrintingQuoteResponse?> GetPrintingQuoteDetailsById(long id)
        {
            return await _cuttingPlanRepository.GetPrintingQuoteDetailsById(id);
        }
        #endregion
    }
}
