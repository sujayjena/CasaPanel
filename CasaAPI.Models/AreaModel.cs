using CasaAPI.Models;
using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class AreaRequest
    {
        public long AreaId { get; set; }
       
        public string AreaName { get; set; }
        public bool IsActive { get; set; }

    }
    public class SearchAreaRequest
    {
        public PaginationParameters pagination { get; set; }
        public string AreaName { get; set; }
        public Nullable<bool> IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }

    public class AreaResponse : CreationDetails
    {
       
        public long AreaId { get; set; }
        public string AreaName { get; set; }
        public bool IsActive { get; set; }       
    }
    public class ImportedAreaDetails
    {
        public string AreaName { get; set; }
        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string RegionName { get; set; }
        public string StateName { get; set; }
        public string IsActive { get; set; }
    }
    public class AreaDataValidationErrors
    {
        public string AreaName { get; set; }
       
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }


    public class SaveAreamapping
    {
        public long Id { get; set; }
        public long AreaId { get; set; }       
        public bool IsActive { get; set; }
        public long RegionId { get; set; }
        public long StateId { get; set; }       
        public long DistrictId { get; set; }
        public long CityId { get; set; }
    }

    public class AreaMappingResponse : CreationDetails
    {
        public long Id { get; set; }
        public long AreaId { get; set; }
        public string AreaName { get; set; }
        public bool IsActive { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }
        public long RegionId { get; set; }
        public string RegionName { get; set; }
        public long DistrictId { get; set; }
        public string DistrictName { get; set; }
        public long CityId { get; set; }
        public string CityName { get; set; }
    }

    public class SearchAreaMappingRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }

    }
}
