using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class ManageBoxSizeModel
    {
        public int BoxSizeId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.TileSize_Dropdown_Msg)]
        public int TileSizeId { get; set; }
        [Required(ErrorMessage = ValidationConstants.NoOfTilesPerBox_Required_Msg)]
        public int NoOfTilesPerBox { get; set; }
        [Required(ErrorMessage = ValidationConstants.WeightPerBox_Required_Msg)]
        public decimal WeightPerBox { get; set; }
        [Required(ErrorMessage = ValidationConstants.Thickness_Required_Msg)]
        public decimal Thickness { get; set; }
        [Required(ErrorMessage = ValidationConstants.BoxCoverageAreaSqFoot_Required_Msg)]
        public decimal BoxCoverageAreaSqFoot { get; set; }
        [Required(ErrorMessage = ValidationConstants.BoxCoverageAreaSqMeter_Required_Msg)]
        public decimal BoxCoverageAreaSqMeter { get; set; }
        public bool IsActive { get; set; }
    }

    public class ManageBoxSizeSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }

    public class ManageBoxSizeResponse : LogParameters
    {
        public int BoxSizeId { get; set; }
        public int TileSizeId { get; set; }
        public string TileSizeName { get; set; }
        public int NoOfTilesPerBox { get; set; }
        public decimal WeightPerBox { get; set; }
        public decimal Thickness { get; set; }
        public decimal BoxCoverageAreaSqFoot { get; set; }
        public decimal BoxCoverageAreaSqMeter { get; set; }
    }

    public class ImportManageBoxSize
    {
        public string TileSizeName { get; set; }
        public int NoOfTilesPerBox { get; set; }
        public decimal WeightPerBox { get; set; }
        public decimal Thickness { get; set; }
        public decimal BoxCoverageAreaSqFoot { get; set; }
        public decimal BoxCoverageAreaSqMeter { get; set; }
        [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
        [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
        public string IsActive { get; set; }
    }

    public class ManageBoxSizeValidationErrors
    {
        public string TileSizeName { get; set; }
        public int NoOfTilesPerBox { get; set; }
        public decimal WeightPerBox { get; set; }
        public decimal Thickness { get; set; }
        public decimal BoxCoverageAreaSqFoot { get; set; }
        public decimal BoxCoverageAreaSqMeter { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
