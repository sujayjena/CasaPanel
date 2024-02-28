using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class PanelDisplaySaveParameters
    {
        public int Id { get; set; }
        public string PanelCode { get; set; }

        public string DesignInfromation { get; set; }
        public int? Collection { get; set; }
        public int? Punch { get; set; }
        public int? Thickness { get; set; }
        public int? Size { get; set; }
      public Decimal?  FullPieceQty { get; set; }
    public Decimal?  CutPieceQty { get; set; }
public bool IsActive { get; set; }
    }

    public class PanelDisplayDetailsResponse : LogParameters
    {
        public int Id { get; set; }

        public string PanelCode { get; set; }

        public string DesignInfromation { get; set; }
        public int Collection { get; set; }
        public int Punch { get; set; }
        public int Thickness { get; set; }
        public int Size { get; set; }

        public string CollectionName { get; set; }
        public string PunchName { get; set; }
        public string ThicknessName { get; set; }
        public string SizeName { get; set; }
        public Decimal FullPieceQty { get; set; }
        public Decimal CutPieceQty { get; set; }

    }
    public class PanelDisplaySearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;

        public bool? IsActive { get; set; }
        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
}
