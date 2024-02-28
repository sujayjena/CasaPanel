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
    public class EmployeeSaveParameters
      {
        public int Id   { get; set; }
       
        public string EmployeeName   { get; set; }
        public string EmployeeCode  { get; set; }
        public decimal MobileNumber  { get; set; }
        public string Email   { get; set; }
        public string Password { get; set; }
        public string Department   { get; set; } 
    public int? Role   { get; set; } 
public int? ReportingTo  { get; set; } 
public DateTime? DateOfBirth  { get; set; } 
public DateTime? DateOfJoining  { get; set; } 
public decimal? EmergencycontactNo  { get; set; } 
public int?   BloodGroup  { get; set; } 
public int? Gender  { get; set; } 
//public int MaterialStatus  { get; set; } 
public string CompanyNaumber  { get; set; } 
public string PermanentAddress  { get; set; } 
public int? PermanentState  { get; set; } 
public int? PermanentRegion  { get; set; }
public int? PermanentDistrict  { get; set; } 
public int? PermanentCity  { get; set; }
public int?    PermanentArea  { get; set; } 
public decimal? PermanentPinCode  { get; set; } 
public bool? IsTemporaryAddressIsSame  { get; set; } 
public string? TemporaryAddress  { get; set; } 
public int? TemporaryState  { get; set; }
public int? TemporaryRegion  { get; set; } 
public int? TemporaryDistrict  { get; set; } 
public int? TemporaryCity  { get; set; } 
public int?   TemporaryArea  { get; set; } 
public decimal? TemporaryPinCode  { get; set; } 
public string EmergencyName  { get; set; } 
public decimal? EmergencyNumber  { get; set; } 
public string EmergencyRelation  { get; set; } 
public string EmployeePostCompanyName  { get; set; } 
public decimal? TotalNumberOfExp  { get; set; } 
public string AddharNumber  { get; set; }
public string UploadAddharCardURL  { get; set; }
        public IFormFile? UploadAddharCardfiles { get; set; }
        public string PANNumber  { get; set; } 
public string OtherProof  { get; set; } 
public string PhotoUploadURL  { get; set; }
        public IFormFile? PhotoUploadfiles { get; set; }
        public string Remark  { get; set; } 
public bool? IsWebUser  { get; set; } 
public bool? IsMobileUser   { get; set; } 
public string ImageUploadURL   { get; set; }
        public bool? IsActive   { get; set; }
        public IFormFile? ImageUploadfiles { get; set; }
    }

    public class EmployeeDetailsResponse : LogParameters
    {
        public int Id { get; set; }
       public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public decimal MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public string Department { get; set; }
        public int Role { get; set; }
        public string RoleName { get; set; }
        public int ReportingTo { get; set; }
        public string ReportingToName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public decimal EmergencycontactNo { get; set; }
        public int BloodGroup { get; set; }
        public string BloodGroupName { get; set; }
        public int Gender { get; set; }
        public string GenderName { get; set; }
       // public int MaterialStatus { get; set; }
        public string CompanyNaumber { get; set; }
        public string PermanentAddress { get; set; }
        public int PermanentState { get; set; }
        public int PermanentRegion { get; set; }
        public int PermanentDistrict { get; set; }
        public int PermanentCity { get; set; }
        public int PermanentArea { get; set; }

        public string PermanentStateName { get; set; }
        public string PermanentRegionName { get; set; }
        public string PermanentDistrictName { get; set; }
        public string PermanentCityName { get; set; }
        public string PermanentAreaName { get; set; }

        public decimal PermanentPinCode { get; set; }
        public bool IsTemporaryAddressIsSame { get; set; }
        public string TemporaryAddress { get; set; }
        public int TemporaryState { get; set; }
        public int TemporaryRegion { get; set; }
        public int TemporaryDistrict { get; set; }
        public int TemporaryCity { get; set; }
        public int TemporaryArea { get; set; }

        public string TemporaryStateName { get; set; }
        public string TemporaryRegionName { get; set; }
        public string TemporaryDistrictName { get; set; }
        public string TemporaryCityName { get; set; }
        public string TemporaryAreaName { get; set; }
        public decimal TemporaryPinCode { get; set; }
        public string EmergencyName { get; set; }
        public decimal EmergencyNumber { get; set; }
        public string EmergencyRelation { get; set; }
        public string EmployeePostCompanyName { get; set; }
        public decimal TotalNumberOfExp { get; set; }
        public string AddharNumber { get; set; }
        public string UploadAddharCard { get; set; }
        public string PANNumber { get; set; }
        public string OtherProof { get; set; }
        public string PhotoUpload { get; set; }
        public string Remark { get; set; }
        public bool IsWebUser { get; set; }
        public bool IsMobileUser { get; set; }
        public string ImageUpload { get; set; }


    }
    public class EmployeeSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
}
