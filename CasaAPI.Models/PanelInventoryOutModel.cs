using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class PanelInventoryOutSaveParameters
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? Desing { get; set; }
        public int? Collection { get; set; }
        public int? Type { get; set; }
        public int? Finish { get; set; }
        public int? Thickness { get; set; }
        public int? CuttingSize { get; set; }
        public DateTime? OutwardingDate { get; set; }
        public int? OutwardingQty { get; set; }
        public decimal? TotalStock { get; set; }
        public bool IsActive { get; set; }
    }
    public class PanelInventoryOutDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? Desing { get; set; }
        public string DesingName { get; set; }
        public int Collection { get; set; }
        public int Type { get; set; }
        public int Finish { get; set; }
        public int Thickness { get; set; }
        public int CuttingSize { get; set; }
        public string CollectionName { get; set; }
        public string TypeName { get; set; }
        public string FinishName { get; set; }
        public string ThicknessName { get; set; }
        public string CuttingSizeName { get; set; }
        public DateTime InwardingDate { get; set; }
        public int InwardingQty { get; set; }
        public decimal TotalStock { get; set; }
    }
    public class PanelInventoryOutSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;

        public bool? IsActive { get; set; }
        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
}
