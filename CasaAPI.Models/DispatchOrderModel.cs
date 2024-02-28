using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    
    public class DispatchOrderSaveParameters
    {
        public int Id { get; set; }
        
public string DONumber { get; set; } = string.Empty;
        public string CSPCode { get; set; } = string.Empty;
        public int? DealerId { get; set; }
        public int? StateId { get; set; }
        //public int? DistrictId { get; set; }
        public int? CityId { get; set; }
       // public int? AreaId { get; set; }
        public string ContactPerson { get; set; }=string.Empty;
        public int? RefBy { get; set; }
        public string CaseDisplayPanel { get; set; } = string.Empty;
        public string CaseFullTileBoxes { get; set; }= string.Empty;
        public string CaseCttingSampleBoxes { get; set; } = string.Empty;
        public int? DispatchedTruckNumber { get; set; }
        public int? DriverId { get; set; }
        public string DriverContactNumber { get; set; } = string.Empty;
        public string LoadingTime { get; set; } = string.Empty;
        public int? MasterName { get; set; }
        public int? MasterNumber { get; set; }
        public string PanelCode { get; set; } = string.Empty;
        public int? Qty { get; set; }

        public bool IsActive { get; set; }
        public List<PanelDisplayList> PanelDisplayLists { get; set; }
    }
    public class PanelDisplayList
    {
        public string PanelCode { get; set; } = string.Empty;
        public string PanelInformation { get; set; } = string.Empty;
    }
    public class DispatchPanelDisplayDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public int DispatchOrderId { get; set; }
        public string PanelCode { get; set; } = string.Empty;
        public string PanelInformation { get; set; } = string.Empty;
    }
        public class DispatchOrderDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public string DONumber { get; set; } 
        public string CSPCode { get; set; }
        public int DealerId { get; set; }
        public int StateId { get; set; }
        public string DealerName { get; set; }
        public string StateName { get; set; }
        //  public int DistrictId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        // public int AreaId { get; set; }
        public string ContactPerson { get; set; } 
        public string RefByName { get; set; }
        public string CaseDisplayPanel { get; set; } 
        public string CaseFullTileBoxes { get; set; } 
        public int DispatchedTruckNumber { get; set; }
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNumber { get; set; }
        public string LoadingTime { get; set; }
        public int MasterName { get; set; }
        public string MasterNames { get; set; }
        public int MasterNumber { get; set; }
        public string PanelCode { get; set; } 
        public int Qty { get; set; }

        public bool IsActive { get; set; }
        public List<PanelDisplayList> PanelDisplayLists { get; set; }
    }
    public class DispatchOrderSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;

        public bool? IsActive { get; set; }
        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
}
