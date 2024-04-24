using CasaAPI.Models.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class ReferralSaveParameters
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = ValidationConstants.RoleNameRequied_Msg)]
        //[RegularExpression(ValidationConstants.RoleNameRegExp, ErrorMessage = ValidationConstants.RoleNameRegExp)]
        //[MaxLength(ValidationConstants.RoleName_MaxLength, ErrorMessage = ValidationConstants.RoleName_MaxLength_Msg)]

        //public string UniqueNo { get; set; }
        public string ReferralParty { get; set; }
        public string Address { get; set; }
        public int State { get; set; }
        public int Region { get; set; }
        public int District { get; set; }
        public int City { get; set; }
        public int Area { get; set; }
        public decimal Pincode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string GstNo { get; set; }
        public string PanNo { get; set; }
        public string AadharFileName { get; set; }
        public string AadharSaveFileName { get; set; }
        public IFormFile AadharFile { get; set; }
        public string PanCardFileName { get; set; }
        public string PanCardSaveFileName { get; set; }
        public IFormFile PanCardFile { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReferralDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public string UniqueNo { get; set; }
        public string ReferralParty { get; set; }
        public string Address { get; set; }
        public int State { get; set; }
        public int Region { get; set; }
        public int District { get; set; }
        public int Area { get; set; }
        public int City { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public string AreaName { get; set; }
        public decimal Pincode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string GstNo { get; set; }
        public string PanNo { get; set; }
        public string AadharFileName { get; set; }
        public string AadharSaveFileName { get; set; }
        public string AadharFileUrl { get; set; }
        public string PanCardFileName { get; set; }
        public string PanCardSaveFileName { get; set; }
        public string PanCardFileUrl { get; set; }
    }
    public class ReferralSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
    public class ReferralImportSaveParameters
    {
        public string UniqueNo { get; set; }
        public string ReferralParty { get; set; }
        public string Address { get; set; }    
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public string AreaName { get; set; }
        public string Pincode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string GstNo { get; set; }
        public string PanNo { get; set; }
        public string IsActive { get; set; }
    }
    public class ReferralFailToImportValidationErrors
    {
        public string UniqueNo { get; set; }
        public string ReferralParty { get; set; }
        public string Address { get; set; }      
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public string AreaName { get; set; }
        public string Pincode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string GstNo { get; set; }
        public string PanNo { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
