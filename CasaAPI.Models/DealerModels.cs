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
    #region Dealer
    public class DealerSaveParameters
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = ValidationConstants.RoleNameRequied_Msg)]
        //[RegularExpression(ValidationConstants.RoleNameRegExp , ErrorMessage = ValidationConstants.RoleNameRegExp)]
        //[MaxLength(ValidationConstants.RoleName_MaxLength, ErrorMessage = ValidationConstants.RoleName_MaxLength_Msg)]
        public string CompanyName { get; set; }
        public string CompanyEmailId { get; set; }
        public string GSTNumber { get; set; }
        public string PANNumber { get; set; }
        public string AadhaarNumber { get; set; }
        public string BusinessCardUpload { get; set; }
        public IFormFile? BusinessCardUploadfiles { get; set; }
        public string CompanyDealingaddress { get; set; }
        public int? State { get; set; }
        public int? Region { get; set; }
        public int? District { get; set; }
        public int? City { get; set; }
        public int? Area { get; set; }
        public decimal? Pincode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ContactType { get; set; }
        public decimal? MobileNumber { get; set; }
        public string EmailId { get; set; }
        public decimal? EmergencyContactNumber { get; set; }
        public DateTime? DateofBirth { get; set; }
        public int? BloodGroup { get; set; }
        public int? Gender { get; set; }

        public DateTime? Anniversarydate { get; set; }
        public string CurrentHandlingBrandOrCompany { get; set; }
        public int? TileType { get; set; }
        public int? SegmentType { get; set; }

        public string AnyOtherBranchDealing { get; set; }
        public string PresentShowroomSqFt { get; set; }
        public string PresentGodownSqFt { get; set; }
        public string DistanceFromShowroomToGodown { get; set; }
        public int? Showroom { get; set; }
        public int? WeekCloseOrOffDayInMarket { get; set; }

        public string SpaceProvidedToCase { get; set; }
        public string ImageUpload { get; set; }
        public IFormFile? ImageUploadfiles { get; set; }
        public string Dealershowroom { get; set; }
        public IFormFile? UploadDealershowroomfiles { get; set; }
        public int? Rating { get; set; }
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }
    }

    public class DealerDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string EmailId { get; set; }
        public string GSTNumber { get; set; }
        public string PANNumber { get; set; }
        public string AadhaarNumber { get; set; }
        public string BusinessCardUpload { get; set; }
        public string CompanyDealingaddress { get; set; }
        public int State { get; set; }
        public int Region { get; set; }
        public int District { get; set; }
        public int City { get; set; }
        public int Area { get; set; }
        public string StateName { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }
        public decimal Pincode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ContactType { get; set; }
        public string ContactTypeName { get; set; }
        public decimal MobileNumber { get; set; }
        public decimal EmergencyContactNumber { get; set; }
        public DateTime DateofBirth { get; set; }
        public int BloodGroup { get; set; }
        public int Gender { get; set; }
        public string BloodGroupName { get; set; }
        public string GenderName { get; set; }
        public DateTime Anniversarydate { get; set; }
        public string CurrentHandlingBrandOrCompany { get; set; }
        public int TileType { get; set; }
        public int SegmentType { get; set; }
        public string TileTypeName { get; set; }
        public string SegmentTypeName { get; set; }
        public string AnyOtherBranchDealing { get; set; }
        public string PresentShowroomSqFt { get; set; }
        public string PresentGodownSqFt { get; set; }
        public string DistanceFromShowroomToGodown { get; set; }
        public int Showroom { get; set; }
        public int WeekCloseOrOffDayInMarket { get; set; }

        public string ShowroomName { get; set; }
        public string WeekCloseOrOffDayInMarketName { get; set; }
        public string SpaceProvidedToCase { get; set; }
        public string ImageUpload { get; set; }
        public string Dealershowroom { get; set; }
        public int Rating { get; set; }
        public int? StatusId { get; set; }
        public string StatusName { get; set; }

    }
    public class DealerSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public int? StatusId { get; set; }
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
    public class DealerStatusUpdate
    {
        public int? Id { get; set; }
        public int? StatusId { get; set; }

    }
    #endregion
    #region DealerAddress
    public class DealerAddressSaveParameters
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string CompanyDealingaddress { get; set; }
        public int? State { get; set; }
        public int? Region { get; set; }
        public int? District { get; set; }
        public int? City { get; set; }
        public int? Area { get; set; }
        public decimal? Pincode { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
    }

    public class DealerAddressDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public string CompanyDealingaddress { get; set; }
        public int DealerId { get; set; }
        public int State { get; set; }
        public int Region { get; set; }
        public int District { get; set; }
        public int City { get; set; }
        public int Area { get; set; }
        public string StateName { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public string CityName { get; set; }
        public string AreaName { get; set; }
        public decimal Pincode { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

    }
    public class DealerAddressSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public int DealerId { get; set; }
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
    #endregion
    #region DealerContactDetails
    public class DealerContactDetailsSaveParameters
    {
        public int Id { get; set; }
        public int DealerId { get; set; }
        public string EmailId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ContactType { get; set; }
        public decimal? MobileNumber { get; set; }
        public decimal? EmergencyContactNumber { get; set; }
        public DateTime? DateofBirth { get; set; }
        public int? BloodGroup { get; set; }
        public int? Gender { get; set; }
        public bool? IsDefault { get; set; }
        public DateTime? Anniversarydate { get; set; }
        public bool? IsActive { get; set; }
    }

    public class DealerContactDetailsResponse : LogParameters
    {

        public int Id { get; set; }
        public int DealerId { get; set; }
        public string EmailId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ContactType { get; set; }
        public string ContactTypeName { get; set; }
        public decimal MobileNumber { get; set; }
        public decimal EmergencyContactNumber { get; set; }
        public DateTime DateofBirth { get; set; }
        public int BloodGroup { get; set; }
        public int Gender { get; set; }
        public string BloodGroupName { get; set; }
        public string GenderName { get; set; }
        public DateTime Anniversarydate { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

    }
    public class DealerContactDetailsSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public int DealerId { get; set; }
        public bool? IsActive { get; set; }
        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
    #endregion 
}
