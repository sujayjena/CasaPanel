using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Services
{
    public interface ICuttingPlanService
    {
        #region Cutting
        Task<int> SaveCuttingPlan(CuttingPlanSaveParameters request);
        Task<IEnumerable<CuttingPlanResponse>> GetCuttingPlanList(SearchCuttingPlanRequest request);
        Task<CuttingPlanResponse?> GetCuttingPlanDetailsById(long id);
        #endregion
        #region QuoteTilesCutting
        Task<int> SaveQuoteTilesCutting(QuoteTilesCuttingSaveParameters request);
        Task<IEnumerable<QuoteTilesCuttingResponse>> GetQuoteTilesCuttingList(SearchQuoteTilesCuttingRequest request);
        Task<QuoteTilesCuttingResponse?> GetQuoteTilesCuttingDetails(long id);
        #endregion

        #region PanelPlanning
        Task<int> SavePanelPlanning(PanelPlanningSaveParameters request);
        Task<IEnumerable<PanelPlanningResponse>> GetPanelPlanningList(SearchPanelPlanningRequest request);
        Task<PanelPlanningResponse?> GetPanelPlanningDetailsById(long id);
        #endregion

        #region QuotePanelDesign
        Task<int> SaveQuotePanelDesign(QuotePanelDesignSaveParameters request);
        Task<IEnumerable<QuotePanelDesignResponse>> GetQuotePanelDesignList(SearchQuotePanelDesignRequest request);
        Task<QuotePanelDesignResponse?> GetQuotePanelDesignDetailsById(long id);
        #endregion

        #region BinderPlanning
        Task<int> SaveBinderPlanning(BinderPlanningSaveParameters request);
        Task<IEnumerable<BinderPlanningResponse>> GetBinderPlanningList(SearchBinderPlanningRequest request);
        Task<BinderPlanningResponse?> GetBinderPlanningDetailsById(long id);
        #endregion
        #region BinderQuote
        Task<int> SaveBinderQuote(BinderQuoteSaveParameters request);
        Task<IEnumerable<BinderQuoteResponse>> GetBinderQuoteList(SearchBinderQuoteRequest request);
        Task<BinderQuoteResponse?> GetBinderQuoteDetailsById(long id);
        #endregion

        #region PrintingPlan
        Task<int> SavePrintingPlan(PrintingPlanSaveParameters request);
        Task<IEnumerable<PrintingPlanResponse>> GetPrintingPlanList(SearchPrintingPlanRequest request);
        Task<PrintingPlanResponse?> GetPrintingPlanDetailsById(long id);
        #endregion

        #region PrintingQuote
        Task<int> SavePrintingQuote(PrintingQuoteSaveParameters request);
        Task<IEnumerable<PrintingQuoteResponse>> GetPrintingQuoteList(SearchPrintingQuoteRequest request);
        Task<PrintingQuoteResponse?> GetPrintingQuoteDetailsById(long id);
        #endregion
    }
}
