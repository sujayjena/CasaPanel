using CasaAPI.Models.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CasaAPI.Models
{
    public class ProductDesignsSaveModel
    {
        public int ProductDesignId { get; set; }

        //[Required(ErrorMessage = ValidationConstants.DesignNameRequied_Msg)]
        //[RegularExpression(ValidationConstants.DesignNameRegExp, ErrorMessage = ValidationConstants.DesignNameRegExp_Msg)]
        //[MaxLength(ValidationConstants.DesignName_MaxLength, ErrorMessage = ValidationConstants.DesignName_MaxLength_Msg)]
        public string DesignName { get; set; }

      //  [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.SizeRequied_Dropdown_Msg)]
        public int? SizeId { get; set; }

       // [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.BrandRequied_Dropdown_Msg)]
        public int? BrandId { get; set; }
        
      //  [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.CollectionRequied_Dropdown_Msg)]
        public int? CollectionId { get; set; }

      //  [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.CategoryRequied_Dropdown_Msg)]
        public int? CategoryId { get; set; }

      //  [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.Type_Dropdown_Msg)]
        public int? TypeId { get; set; }

       // [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.Punch_Dropdown_Msg)]
        public int? PunchId { get; set; }

      //  [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.Surface_Dropdown_Msg)]
        public int? SurfaceId { get; set; }

      //  [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.Thickness_Dropdown_Msg)]
        public int? ThicknessId { get; set; }

       // [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.TileSize_Dropdown_Msg)]
        public int? TileSizeId { get; set; }
        
       // [Required(ErrorMessage = ValidationConstants.NoOfTilesPerBox_Required_Msg)]
        public int? NoOfTilesPerBox { get; set; }

       // [Required(ErrorMessage = ValidationConstants.WeightPerBox_Required_Msg)]
        public decimal? WeightPerBox { get; set; }

       // [Required(ErrorMessage = ValidationConstants.BoxCoverageAreaSqFoot_Required_Msg)]
        public decimal? BoxCoverageAreaSqFoot { get; set; }

        //[Required(ErrorMessage = ValidationConstants.BoxCoverageAreaSqMeter_Required_Msg)]
        public decimal? BoxCoverageAreaSqMeter { get; set; }
        public bool IsActive { get; set; }

        public string FinishName { get; set; }

        public List<IFormFile>? DesignFiles { get; set; }
        public List<ProductDesignFiles> ProductDesignFiles { get; set; }

    }
    
    public class ProductDesignFiles
    {
        public int? ProductDesignFilesId { get; set; }
        public int? ProductDesignId { get; set; }
       // [RegularExpression(ValidationConstants.ImageFileRegExp, ErrorMessage = ValidationConstants.ImageFileRegExp_Msg)]
        public string UploadedFilesName { get; set; }
        public string StoredFilesName { get; set; }
        public string FileType { get; set; }
        public string DesignFile { get; set; }
        //[JsonIgnore, XmlIgnore]
       // public IFormFile DesignFile { get; set; }
    }

    public class ProductDesignFilesResponse : ProductDesignFiles
    {
        public byte[] FileContent { get; set; }
    }

    public class ProductDesignResponse : LogParameters
    {
        public int ProductDesignId { get; set; }
        public string DesignName { get; set; }
        public string SizeName { get; set; }
        public string BrandName { get; set; }
        public string CollectionName { get; set; }
        public string CategoryName { get; set; }
        public string TypeName { get; set; }
        public string PunchName { get; set; }
        public string SurfaceName { get; set; }
        public string ThicknessName { get; set; }
        public string TileSizeName { get; set; }
        public int NoOfTilesPerBox { get; set; }
        public decimal WeightPerBox { get; set; }
        public decimal BoxCoverageAreaSqFoot { get; set; }
        public decimal BoxCoverageAreaSqMeter { get; set; }
        public string FinishName { get; set; }
    }

    public class ProductDesignDetailsResponse : ProductDesignResponse
    {
        public int SizeId { get; set; }
        public int BrandId { get; set; }
        public int CollectionId { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int PunchId { get; set; }
        public int SurfaceId { get; set; }
        public int ThicknessId { get; set; }
        public int TileSizeId { get; set; }
        public List<ProductDesignFilesResponse> ProductDesignFilesList { get; set; }
    }

    public class ProductDesignSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }

    public class ImportProductDesign
    {
        public string DesignName { get; set; }
        public string SizeName { get; set; }
        public string BrandName { get; set; }
        public string CollectionName { get; set; }
        public string CategoryName { get; set; }
        public string TypeName { get; set; }
        public string PunchName { get; set; }
        public string SurfaceName { get; set; }
        public string ThicknessName { get; set; }
        public string TileSizeName { get; set; }
        public int NoOfTilesPerBox { get; set; }
        public decimal WeightPerBox { get; set; }
        public decimal BoxCoverageAreaSqFoot { get; set; }
        public decimal BoxCoverageAreaSqMeter { get; set; }
        [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
        [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]

        public string FinishName { get; set; }
        public string IsActive { get; set; }
    }

    public class ProductDesignValidationErrors
    {
        public string DesignName { get; set; }
        public string SizeName { get; set; }
        public string BrandName { get; set; }
        public string CollectionName { get; set; }
        public string CategoryName { get; set; }
        public string TypeName { get; set; }
        public string PunchName { get; set; }
        public string SurfaceName { get; set; }
        public string ThicknessName { get; set; }
        public string TileSizeName { get; set; }
        public int NoOfTilesPerBox { get; set; }
        public decimal WeightPerBox { get; set; }
        public decimal BoxCoverageAreaSqFoot { get; set; }
        public decimal BoxCoverageAreaSqMeter { get; set; }
        public string FinishName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
