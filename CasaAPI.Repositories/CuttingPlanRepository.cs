using CasaAPI.Helpers;
using CasaAPI.Interfaces.Repositories;
using CasaAPI.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static CasaAPI.Models.DriverDeatilsModel;

namespace CasaAPI.Repositories
{
    public class CuttingPlanRepository: BaseRepository, ICuttingPlanRepository
    {
        private IConfiguration _configuration;
        public CuttingPlanRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region Cutting Plan
        public async Task<int> SaveCuttingPlan(CuttingPlanSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@PlanId", parameters.PlanId);
      queryParameters.Add("@CatDate", parameters?.CatDate);
      queryParameters.Add("@Collection", parameters?.Collection);
      queryParameters.Add("@Design",parameters?.Design);
      queryParameters.Add("@Type",parameters?.Types);
      queryParameters.Add("@Finish",parameters?.Finish);
      queryParameters.Add("@Thickness",parameters?.Thickness);
      queryParameters.Add("@FullPieceqty",parameters?.FullPieceqty);
      queryParameters.Add("@FullSize",parameters?.FullSize);
      queryParameters.Add("@CutPieceqty",parameters?.CutPieceqty);
      queryParameters.Add("@CutSize",parameters?.CutSize);
      queryParameters.Add("@TotalBox",parameters?.TotalBox);
      queryParameters.Add("@TotalPcsPerbox",parameters?.TotalPcsPerbox);
      queryParameters.Add("@Pieces",parameters?.Pieces);
      queryParameters.Add("@TotalPieces",parameters?.TotalPieces);
      queryParameters.Add("@Cuttingsize",parameters?.Cuttingsize);
      queryParameters.Add("@CuttingPicesFrom1Tile",parameters?.CuttingPicesFrom1Tile);
      queryParameters.Add("@WastageCutPieces",parameters?.WastageCutPieces);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveCuttingPlanDetails", queryParameters);
        }
        public async Task<IEnumerable<CuttingPlanResponse>> GetCuttingPlanList(SearchCuttingPlanRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<CuttingPlanResponse>("GetCuttingPlanList", queryParameters);
        }
        public async Task<CuttingPlanResponse?> GetCuttingPlanDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<CuttingPlanResponse>("GetCuttingPlanDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region QuoteTilesCutting
        public async Task<int> SaveQuoteTilesCutting(QuoteTilesCuttingSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@VendorType", parameters.VendorType);
queryParameters.Add("@CuttingVendor", parameters.CuttingVendor);
queryParameters.Add("@CuttingRatePerPiece", parameters.CuttingRatePerPiece);
queryParameters.Add("@TotalCuttingQty", parameters.TotalCuttingQty);
queryParameters.Add("@SubAmount", parameters.SubAmount);
queryParameters.Add("@FreightChargesCompany", parameters.FreightChargesCompany);
queryParameters.Add("@FreightCharges", parameters.FreightCharges);
queryParameters.Add("@TotalAmount", parameters.TotalAmount);
queryParameters.Add("@Reference", parameters.Reference);
queryParameters.Add("@DivideByInch", parameters.DivideByInch);
queryParameters.Add("@CuttingCompany", parameters.CuttingCompany);
queryParameters.Add("@Remark", parameters.Remark);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveQuoteTilesCuttingDetails", queryParameters);
        }
        public async Task<IEnumerable<QuoteTilesCuttingResponse>> GetQuoteTilesCuttingList(SearchQuoteTilesCuttingRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<QuoteTilesCuttingResponse>("GetQuoteTilesCuttingList", queryParameters);
        }
        public async Task<QuoteTilesCuttingResponse?> GetQuoteTilesCuttingDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<QuoteTilesCuttingResponse>("GetQuoteTilesCuttingDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region PanelPlanning
        public async Task<int> SavePanelPlanning(PanelPlanningSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@PlanId", parameters.TypeOfPanel);
            queryParameters.Add("@MDFThickness", parameters?.MDFThickness);
            queryParameters.Add("@MDFSize", parameters?.MDFSize);
            queryParameters.Add("@HSNCode", parameters?.HSNCode);
            queryParameters.Add("@CuttingPlanId", parameters?.CuttingPlanId);
            queryParameters.Add("@Finish", parameters?.Finish);
            queryParameters.Add("@Thickness", parameters?.Thickness);
            queryParameters.Add("@Cpllection", parameters?.Cpllection);
            queryParameters.Add("@Design", parameters?.Design);
            queryParameters.Add("@Type", parameters?.Types);
            queryParameters.Add("@Fullpieceqty", parameters?.Fullpieceqty);
            queryParameters.Add("@Size", parameters?.Size);
            queryParameters.Add("@CutPieceQty", parameters?.CutPieceQty);
            queryParameters.Add("@CutSize", parameters?.CutSize);
            queryParameters.Add("@ImageUpload", parameters?.ImageUpload);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePanelPlanningDetails", queryParameters);
        }
        public async Task<IEnumerable<PanelPlanningResponse>> GetPanelPlanningList(SearchPanelPlanningRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<PanelPlanningResponse>("GetPanelPlanningList", queryParameters);
        }
        public async Task<PanelPlanningResponse?> GetPanelPlanningDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PanelPlanningResponse>("GetPanelPlanningDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region QuotePanelDesign
        public async Task<int> SaveQuotePanelDesign(QuotePanelDesignSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@PlanId", parameters.QuotePPId);
            queryParameters.Add("@CatDate", parameters?.RatePerPanel);
            queryParameters.Add("@Collection", parameters?.Totalquantity);
            queryParameters.Add("@Design", parameters?.FrieghtTransportCharges);
            queryParameters.Add("@Type", parameters?.PanelMositure);
            queryParameters.Add("@Finish", parameters?.VendorType);
            queryParameters.Add("@Thickness", parameters?.VendorCompany);
            queryParameters.Add("@FullPieceqty", parameters?.Remark);
            queryParameters.Add("@FullSize", parameters?.Packing);
            queryParameters.Add("@CutPieceqty", parameters?.RatePerSqFt);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveQuotePanelDesignDetails", queryParameters);
        }
        public async Task<IEnumerable<QuotePanelDesignResponse>> GetQuotePanelDesignList(SearchQuotePanelDesignRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<QuotePanelDesignResponse>("GetQuotePanelDesignList", queryParameters);
        }
        public async Task<QuotePanelDesignResponse?> GetQuotePanelDesignDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<QuotePanelDesignResponse>("GetQuotePanelDesignDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region BinderPlanning
        public async Task<int> SaveBinderPlanning(BinderPlanningSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@Collection", parameters?.Collection);
queryParameters.Add("@FoId", parameters?.FoId);
queryParameters.Add("@NoOfpieces", parameters?.NoOfpieces);
queryParameters.Add("@Thickness", parameters?.Thickness);
queryParameters.Add("@BinderSize", parameters?.BinderSize);
queryParameters.Add("@SizeInch", parameters?.SizeInch);
queryParameters.Add("@SizeFoot", parameters?.SizeFoot);
queryParameters.Add("@SizeCM", parameters?.SizeCM);
queryParameters.Add("@SizeMM", parameters?.SizeMM);
queryParameters.Add("@Flap", parameters?.Flap);
queryParameters.Add("@TitleGSM", parameters?.TitleGSM);
queryParameters.Add("@FlapGSM", parameters?.FlapGSM);
queryParameters.Add("@InnerGSM", parameters?.InnerGSM);
queryParameters.Add("@NoOfColorPrinting", parameters?.NoOfColorPrinting);
queryParameters.Add("@TitleProcess", parameters?.TitleProcess);
queryParameters.Add("@MattLimitation", parameters?.MattLimitation);
queryParameters.Add("@Canvas", parameters?.Canvas);
queryParameters.Add("@Qty", parameters?.Qty);
queryParameters.Add("@delivery", parameters?.delivery);
queryParameters.Add("@Remark", parameters?.Remark);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveBinderPlanningDetails", queryParameters);
        }
        public async Task<IEnumerable<BinderPlanningResponse>> GetBinderPlanningList(SearchBinderPlanningRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<BinderPlanningResponse>("GetBinderPlanningList", queryParameters);
        }
        public async Task<BinderPlanningResponse?> GetBinderPlanningDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<BinderPlanningResponse>("GetBinderPlanningDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region BinderQuote
        public async Task<int> SaveBinderQuote(BinderQuoteSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@Packing", parameters?.Packing);
            queryParameters.Add("@RatePerPanel", parameters?.RatePerPanel);
            queryParameters.Add("@TotalQuantity", parameters?.TotalQuantity);
            queryParameters.Add("@VendorType", parameters?.VendorType);
            queryParameters.Add("@Vendor", parameters?.Vendor);
            queryParameters.Add("@RatePersqFt", parameters?.RatePersqFt);
            queryParameters.Add("@Remark", parameters?.Remark);
            queryParameters.Add("@TransportRate", parameters?.TransportRate);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SaveBinderQuoteDetails", queryParameters);
        }
        public async Task<IEnumerable<BinderQuoteResponse>> GetBinderQuoteList(SearchBinderQuoteRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<BinderQuoteResponse>("GetBinderQuoteList", queryParameters);
        }
        public async Task<BinderQuoteResponse?> GetBinderQuoteDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<BinderQuoteResponse>("GetBinderQuoteDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region PrintingPlan
        public async Task<int> SavePrintingPlan(PrintingPlanSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@PrintingType", parameters.PrintingType);
queryParameters.Add("@Page", parameters.Page);
queryParameters.Add("@SizeInch", parameters.SizeInch);
queryParameters.Add("@SizeFoot", parameters.SizeFoot);
queryParameters.Add("@SizeCM", parameters.SizeCM);
queryParameters.Add("@SizeMM", parameters.SizeMM);
queryParameters.Add("@Qty", parameters.Qty);
queryParameters.Add("@TitleGSM", parameters.TitleGSM);
queryParameters.Add("@InnerGSM", parameters.InnerGSM);
queryParameters.Add("@PaperGsm", parameters.PaperGsm);
queryParameters.Add("@ColorPrinting", parameters.ColorPrinting);
queryParameters.Add("@Other", parameters.Other);
queryParameters.Add("@Remarks", parameters.Remarks);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePrintingPlanDetails", queryParameters);
        }
        public async Task<IEnumerable<PrintingPlanResponse>> GetPrintingPlanList(SearchPrintingPlanRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<PrintingPlanResponse>("GetPrintingPlanList", queryParameters);
        }
        public async Task<PrintingPlanResponse?> GetPrintingPlanDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PrintingPlanResponse>("GetPrintingPlanDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion

        #region PrintingQuote
        public async Task<int> SavePrintingQuote(PrintingQuoteSaveParameters parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@Delievery", parameters?.Delievery);
            queryParameters.Add("@Vendor", parameters?.Vendor);
            queryParameters.Add("@OrderQty", parameters?.OrderQty);
            queryParameters.Add("@VendorType", parameters?.VendorType);
            queryParameters.Add("@UnitRate", parameters?.UnitRate);
            queryParameters.Add("@Rate", parameters?.Rate);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@LoggedInUserId", SessionManager.LoggedInUserId);
            return await SaveByStoredProcedure<int>("SavePrintingQuoteDetails", queryParameters);
        }
        public async Task<IEnumerable<PrintingQuoteResponse>> GetPrintingQuoteList(SearchPrintingQuoteRequest parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@PageNo", parameters.pagination.PageNo);
            queryParameters.Add("@PageSize", parameters.pagination.PageSize);
            queryParameters.Add("@SortBy", parameters.pagination.SortBy.SanitizeValue());
            queryParameters.Add("@OrderBy", parameters.pagination.OrderBy.SanitizeValue());
            queryParameters.Add("@ValueForSearch", parameters.ValueForSearch.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);

            return await ListByStoredProcedure<PrintingQuoteResponse>("GetPrintingQuoteList", queryParameters);
        }
        public async Task<PrintingQuoteResponse?> GetPrintingQuoteDetailsById(long id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", id);

            return (await ListByStoredProcedure<PrintingQuoteResponse>("GetPrintingQuoteDetailsById", queryParameters)).FirstOrDefault();
        }
        #endregion
    }
}
