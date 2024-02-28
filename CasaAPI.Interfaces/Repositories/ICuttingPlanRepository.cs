using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Repositories
{
    public interface ICuttingPlanRepository
    {
        #region Cutting
        Task<int> SaveCuttingPlan(CuttingPlanSaveParameters parameters);
        Task<IEnumerable<CuttingPlanResponse>> GetCuttingPlanList(SearchCuttingPlanRequest parameters);
        Task<CuttingPlanResponse?> GetCuttingPlanDetailsById(long id);
        #endregion
        #region QuoteTilesCutting
        Task<int> SaveQuoteTilesCutting(QuoteTilesCuttingSaveParameters parameters);
        Task<IEnumerable<QuoteTilesCuttingResponse>> GetQuoteTilesCuttingList(SearchQuoteTilesCuttingRequest parameters);
        Task<QuoteTilesCuttingResponse?> GetQuoteTilesCuttingDetailsById(long id);
        #endregion

        #region PanelPlanning
        Task<int> SavePanelPlanning(PanelPlanningSaveParameters parameters);
        Task<IEnumerable<PanelPlanningResponse>> GetPanelPlanningList(SearchPanelPlanningRequest parameters);
        Task<PanelPlanningResponse?> GetPanelPlanningDetailsById(long id);
        #endregion

        #region QuotePanelDesign
        Task<int> SaveQuotePanelDesign(QuotePanelDesignSaveParameters parameters);
        Task<IEnumerable<QuotePanelDesignResponse>> GetQuotePanelDesignList(SearchQuotePanelDesignRequest parameters);
        Task<QuotePanelDesignResponse?> GetQuotePanelDesignDetailsById(long id);
        #endregion

        #region BinderPlanning
        Task<int> SaveBinderPlanning(BinderPlanningSaveParameters parameters);
        Task<IEnumerable<BinderPlanningResponse>> GetBinderPlanningList(SearchBinderPlanningRequest parameters);
        Task<BinderPlanningResponse?> GetBinderPlanningDetailsById(long id);
        #endregion

        #region BinderQuote
        Task<int> SaveBinderQuote(BinderQuoteSaveParameters parameters);
        Task<IEnumerable<BinderQuoteResponse>> GetBinderQuoteList(SearchBinderQuoteRequest parameters);
        Task<BinderQuoteResponse?> GetBinderQuoteDetailsById(long id);
        #endregion

        #region PrintingPlan
        Task<int> SavePrintingPlan(PrintingPlanSaveParameters parameters);
        Task<IEnumerable<PrintingPlanResponse>> GetPrintingPlanList(SearchPrintingPlanRequest parameters);
        Task<PrintingPlanResponse?> GetPrintingPlanDetailsById(long id);
        #endregion

        #region PrintingQuote
        Task<int> SavePrintingQuote(PrintingQuoteSaveParameters parameters);
        Task<IEnumerable<PrintingQuoteResponse>> GetPrintingQuoteList(SearchPrintingQuoteRequest parameters);
        Task<PrintingQuoteResponse?> GetPrintingQuoteDetailsById(long id);
        #endregion
    }
}
