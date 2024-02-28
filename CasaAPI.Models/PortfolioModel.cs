using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class PortfolioSaveParameters
    {
        public int Id { get; set; }
        public int? CompanyId{ get; set; }
        public int? DesignId { get; set; }
        public int? CollectionId { get; set; }
        public int? CatregoryId { get; set; }
        public int? TypeId { get; set; }
        public int? PunchId { get; set; }
        public int? SurfaceId { get; set; }
        public int? BrandId { get; set; }
        public int? TileSizeId { get; set; }
        public string NoOfTilesPerBox { get; set; }
        public string WeightPerBox { get; set; }
        public int? ThicknessId { get; set; }
        public string CoverageAreaperBoxFoot { get; set; }
        public string CoverageAreaPerBoxMerte { get; set; }
        public string ImageUpload { get; set; }
        public IFormFile? ImageUploadfile { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }

    }
    public class PortfolioDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string DesignName { get; set; }
        public int DesignId { get; set; }
        public int CompanyId { get; set; }
        public int CollectionId { get; set; }
        public int CatregoryId { get; set; }
        public int TypeId { get; set; }
        public int PunchId { get; set; }
        public int SurfaceId { get; set; }
        public int BrandId { get; set; }
        public int TileSizeId { get; set; }
        public int ThicknessId { get; set; }
        public string CollectionName { get; set; }
        public string Catregory { get; set; }
        public string Type { get; set; }
        public string PunchName { get; set; }
        public string Surface { get; set; }
        public string Brand { get; set; }
        public string TileSize { get; set; }
        public string NoOfTilesPerBox { get; set; }
        public string WeightPerBox { get; set; }
        public string Thickness { get; set; }
        public string CoverageAreaperBoxFoot { get; set; }
        public string CoverageAreaPerBoxMerte { get; set; }
        public string ImageUpload { get; set; }
        public string Remarks { get; set; }
    }
    public class PortfolioSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;

        public bool? IsActive { get; set; }
        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
}
