using CasaAPI.Models;
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
    public class CityRequest
    {
        public long CityId { get; set; }
    
        public string CityName { get; set; }
        public bool IsActive { get; set; }
    }
    public class SearchCityRequest
    {
        public PaginationParameters pagination { get; set; }
        public string CityName { get; set; }
        public Nullable<bool> IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }

    public class CityResponse : CreationDetails
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
      
        public bool IsActive { get; set; }
       
    }
    public class ImportedCityDetails
    {
        public string CityName { get; set; }
      
        public string IsActive { get; set; }
    }
    public class CityDataValidationErrors
    {
        public string CityName { get; set; }
       
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
