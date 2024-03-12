using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    #region CuttingPlan
    public class CuttingPlanSaveParameters
    {
        public long Id { get; set; }
        public string PlanId { get; set; }
        public DateTime? CatDate { get; set; }
        public long? CollectionId { get; set; }
        public long? DesignId { get; set; }
        public long? TypeId { get; set; }
        public long? FinishId { get; set; }
        public long? ThicknessId { get; set; }
        public decimal? FullPieceqty { get; set; }
        public string FullSize { get; set; }
        public decimal? CutPieceqty { get; set; }
        public string CutSize { get; set; }
        public decimal? TotalBox { get; set; }
        public string TotalPcsPerbox { get; set; }
        public decimal? Pieces { get; set; }
        public decimal? TotalPieces { get; set; }
        public string Cuttingsize { get; set; }
        public string CuttingPicesFrom1Tile { get; set; }
        public decimal? WastageCutPieces  { get; set; }
        public bool IsActive { get; set; }

    }
    public class SearchCuttingPlanRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class CuttingPlanResponse : CreationDetails
    {

        public long Id { get; set; }
        public string PlanId { get; set; }
        public DateTime CatDate { get; set; }
        public long CollectionId { get; set; }
        public long DesignId { get; set; }
        public long TypeId { get; set; }
        public long FinishId { get; set; }
        public long ThicknessId { get; set; }

        public string CollectionName { get; set; }
        public string DesignName { get; set; }
        public string TypeName { get; set; }
        public string FinishName { get; set; }
        public string ThicknessName { get; set; }

        public decimal FullPieceqty { get; set; }
        public string FullSize { get; set; }
        public decimal CutPieceqty { get; set; }
        public string CutSize { get; set; }
        public decimal TotalBox { get; set; }
        public string TotalPcsPerbox { get; set; }
        public decimal Pieces { get; set; }
        public decimal TotalPieces { get; set; }
        public string Cuttingsize { get; set; }
        public string CuttingPicesFrom1Tile { get; set; }
        public decimal WastageCutPieces { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion

    #region QuoteTilesCutting

    public class QuoteTilesCuttingSaveParameters
    {
        public long Id { get; set; }
        public long? VendorTypeId { get; set; }
        public long? CuttingVendorId { get; set; }
        public decimal? CuttingRatePerPiece { get; set; }
        public long? TotalCuttingQty { get; set; }
        public decimal? SubAmount { get; set; }
        public decimal? FreightChargesCompany { get; set; }
        public decimal? FreightCharges { get; set; }
        public decimal? TotalAmount { get; set; }
        public long? ReferenceId { get; set; }
        public string? DivideByInch { get; set; }
        public string CuttingCompany { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
    }
    public class SearchQuoteTilesCuttingRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class QuoteTilesCuttingResponse : CreationDetails
    {
        public long Id { get; set; }
        public long VendorTypeId { get; set; }
        public string VendorType { get; set; }
        public long CuttingVendorId { get; set; }
        public string VendorName { get; set; }
        public decimal CuttingRatePerPiece { get; set; }
        public long TotalCuttingQty { get; set; }
        public decimal SubAmount { get; set; }
        public decimal FreightChargesCompany { get; set; }
        public decimal FreightCharges { get; set; }
        public decimal TotalAmount { get; set; }
        public long ReferenceId { get; set; }
        public string ReferenceName { get; set; }
        public string DivideByInch { get; set; }
        public string CuttingCompany { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
    }

    #endregion

    #region PanelPlanning
    public class PanelPlanningSaveParameters
    {
        public long Id { get; set; }

        public long? TypeOfPanelId { get; set; }
        public string MDFThickness { get; set; }
        public string MDFSize { get; set; }
        public string HSNCode { get; set; }
        public long? CuttingPlanId { get; set; }
        public long? CollectionId { get; set; }
        public long? DesignId { get; set; }
        public long? TypeId { get; set; }
        public long? FinishId { get; set; }
        public long? ThicknessId { get; set; }
        public decimal? Fullpieceqty { get; set; }
        public string Size { get; set; }
        public string CutPieceQty { get; set; }
        public string CutSize { get; set; }
        public string ImageUpload { get; set; }
        public IFormFile? ImageUploadFile { get; set; }
        public bool IsActive { get; set; }

    }
    public class SearchPanelPlanningRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class PanelPlanningResponse : CreationDetails
    {

        public long Id { get; set; }
        public long TypeOfPanelId { get; set; }
        public string PanelTypeName { get; set; }

        public string MDFThickness { get; set; }
        public string MDFSize { get; set; }
        public string HSNCode { get; set; }
        public long CuttingPlanId { get; set; }

        public long CollectionId { get; set; }
        public string CollectionName { get; set; }

        public long DesignId { get; set; }
        public string DesignName { get; set; }

        public long TypeId { get; set; }
        public string TypeName { get; set; }

        public long FinishId { get; set; }
        public string FinishName { get; set; }

        public long ThicknessId { get; set; }
        public string ThicknessName { get; set; }

        public decimal Fullpieceqty { get; set; }
        public string Size { get; set; }
        
        public string CutPieceQty { get; set; }
        public string CutSize { get; set; }
        public string ImageUpload { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion

    #region QuotePanelDesign
    public class QuotePanelDesignSaveParameters
    {
        public long Id { get; set; }
        public long? PanelPlanningId { get; set; }
        public long? VendorTypeId { get; set; }
        public long? ManageVenderId { get; set; }
        public decimal RatePerPanel { get; set; }
        public decimal RatePerSqFt { get; set; }
        public decimal Totalquantity { get; set; }
        public decimal FrieghtTransportCharges { get; set; }
        public bool? Packing { get; set; }
        public string Remark { get; set; }
        public string PanelMositure { get; set; }
        public bool IsActive { get; set; }

    }
    public class SearchQuotePanelDesignRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class QuotePanelDesignResponse : CreationDetails
    {

        public long Id { get; set; }
        public long PanelPlanningId { get; set; }
        public long VendorTypeId { get; set; }
        public string VendorType { get; set; }
        public long ManageVenderId { get; set; }
        public string VendorName { get; set; }
        public decimal RatePerPanel { get; set; }
        public decimal RatePerSqFt { get; set; }
        public decimal Totalquantity { get; set; }
        public decimal FrieghtTransportCharges { get; set; }
        public bool Packing { get; set; }
        public string Remark { get; set; }
        public string PanelMositure { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion

    #region BinderPlanning
    public class BinderPlanningSaveParameters
    {
        public long Id { get; set; }       
        public long? Collection { get; set; }
        public long? FoId { get; set; }
        public String NoOfpieces { get; set; }
        public long? Thickness { get; set; }
        public String BinderSize { get; set; }
        public String SizeInch { get; set; }
        public String SizeFoot { get; set; }
        public String SizeCM { get; set; }
        public String SizeMM { get; set; }
        public long? Flap { get; set; }
        public long? TitleGSM { get; set; }
        public long? FlapGSM { get; set; }
        public long? InnerGSM { get; set; }
        public long? NoOfColorPrinting { get; set; }
        public long? TitleProcess { get; set; }
        public bool? MattLimitation { get; set; }
        public bool? Canvas { get; set; }
        public long? Qty { get; set; }
        public long? delivery { get; set; }
        public String Remark { get; set; }
        public bool IsActive { get; set; }

    }
    public class SearchBinderPlanningRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class BinderPlanningResponse : CreationDetails
    {
        public long Id { get; set; }
        public long Collection { get; set; }
        public long FoId { get; set; }
        public string CollectionName { get; set; }
        public String NoOfpieces { get; set; }
        public long Thickness { get; set; }
        public string ThicknessName { get; set; }
        public String BinderSize { get; set; }
        public String SizeInch { get; set; }
        public String SizeFoot { get; set; }
        public String SizeCM { get; set; }
        public String SizeMM { get; set; }
        public long Flap { get; set; }
        public long TitleGSM { get; set; }
        public long FlapGSM { get; set; }
        public long InnerGSM { get; set; }
        public long NoOfColorPrinting { get; set; }
        public long TitleProcess { get; set; }
        public bool MattLimitation { get; set; }
        public bool Canvas { get; set; }
        public long Qty { get; set; }
        public long delivery { get; set; }
        public String Remark { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion

    #region BinderQuote
    public class BinderQuoteSaveParameters
    {
        public long Id { get; set; }
        public long VendorType { get; set; }
        public long Vendor { get; set; }
        public string RatePerPanel { get; set; }
        public string RatePersqFt { get; set; }
        public long TotalQuantity { get; set; }
        public string TransportRate { get; set; }
        public bool Packing { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }

    }
    public class SearchBinderQuoteRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class BinderQuoteResponse : CreationDetails
    {

        public long Id { get; set; }
        public long VendorType { get; set; }
        public long Vendor { get; set; }
        public string VendorTypeName { get; set; }
        public string VendorName { get; set; }
        public string RatePerPanel { get; set; }
        public string RatePersqFt { get; set; }
        public long TotalQuantity { get; set; }
        public string TransportRate { get; set; }
        public bool Packing { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }
    }
    #endregion

    #region PrintingPlan
    public class PrintingPlanSaveParameters
    {
        public long Id { get; set; }
        public long PrintingType { get; set; }
        public string Page { get; set; }
        public string SizeInch { get; set; }
        public string SizeFoot { get; set; }
        public string SizeCM { get; set; }
        public string SizeMM { get; set; }
        public long Qty { get; set; }
        public long TitleGSM { get; set; }
        public long InnerGSM { get; set; }
        public long PaperGsm { get; set; }
        public long ColorPrinting { get; set; }
        public string Other { get; set; }
        public string Remarks { get; set; }


        public bool IsActive { get; set; }

    }
    public class SearchPrintingPlanRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class PrintingPlanResponse : CreationDetails
    {

        public long Id { get; set; }
        public long PrintingType { get; set; }
        public string Page { get; set; }
        public string SizeInch { get; set; }
        public string SizeFoot { get; set; }
        public string SizeCM { get; set; }
        public string SizeMM { get; set; }
        public long Qty { get; set; }
        public long TitleGSM { get; set; }
        public long InnerGSM { get; set; }
        public long PaperGsm { get; set; }
        public long ColorPrinting { get; set; }
        public string Other { get; set; }
        public string Remarks { get; set; }

        public bool IsActive { get; set; }
    }
    #endregion

    #region PrintingQuote
    public class PrintingQuoteSaveParameters
    {
        public long Id { get; set; }
        public long VendorType { get; set; }
        public long Vendor { get; set; }
        public string Delievery { get; set; }
        public long OrderQty { get; set; }
        public decimal Rate { get; set; }
        public decimal UnitRate { get; set; }
        public bool IsActive { get; set; }

    }
    public class SearchPrintingQuoteRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class PrintingQuoteResponse : CreationDetails
    {

        public long Id { get; set; }
        public long VendorType { get; set; }
        public long Vendor { get; set; }
        public long VendorTypeName { get; set; }
        public long VendorName { get; set; }
        public string Delievery { get; set; }
        public long OrderQty { get; set; }
        public decimal Rate { get; set; }
        public decimal UnitRate { get; set; }

        public bool IsActive { get; set; }
    }
    #endregion
}
