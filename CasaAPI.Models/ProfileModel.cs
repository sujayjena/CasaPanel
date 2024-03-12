﻿using CasaAPI.Models;
using CasaAPI.Models.Constants;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CasaAPI.Models
{
    public class RoleRequest
    {
        public long RoleId { get; set; }
        [Required(ErrorMessage = ValidationConstants.RoleNameRequied_Msg)]
        [RegularExpression(ValidationConstants.RoleNameRegExp, ErrorMessage = ValidationConstants.RoleNameRegExp_Msg)]
        [MaxLength(ValidationConstants.RoleName_MaxLength, ErrorMessage = ValidationConstants.RoleName_MaxLength_Msg)]
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }


    public class SearchRoleRequest
    {
        public PaginationParameters pagination { get; set; }
        public string RoleName { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
    public class RoleResponse : CreationDetails
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class ImportedRoleDetails
    {
        public string RoleName { get; set; }
        public string IsActive { get; set; }
    }

    public class RoleDataValidationErrors
    {
        public string RoleName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }

    public class ReportingToRequest
    {
        public long Id { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.RoleRequied_Dropdown_Msg)]
        public long RoleId { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.ReportingToRequied_Dropdown_Msg)]
        public long ReportingTo { get; set; }
        public bool IsActive { get; set; }
    }
    public class ReportingToResponse
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long ReportingTo { get; set; }
        public bool IsActive { get; set; }
        public string CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ReportingToName { get; set; }
    }

    public class SearchReportingToRequest
    {
        public PaginationParameters pagination { get; set; }
        public long ReportingTo { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
    public class ImportedReportingToDetails
    {
        public string RoleName { get; set; }
        public string ReportingToName { get; set; }
        public string IsActive { get; set; }
    }

    public class ReportingToDataValidationErrors
    {
        public string RoleName { get; set; }
        public string ReportingToName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
    public class SearchEmployeeRequest
    {
        public PaginationParameters pagination { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }

    public class EmployeeRequest
    {
        public long EmployeeId { get; set; }

        [Required(ErrorMessage = ValidationConstants.EmployeeNameRequied_Msg)]
        [RegularExpression(ValidationConstants.EmployeeNameRegExp, ErrorMessage = ValidationConstants.EmployeeNameRegExp_Msg)]
        [MaxLength(ValidationConstants.EmployeeName_MaxLength, ErrorMessage = ValidationConstants.EmployeeName_MaxLength_Msg)]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = ValidationConstants.EmpCodeRequired_Msg)]
        [RegularExpression(ValidationConstants.EmpCodeRegExp, ErrorMessage = ValidationConstants.EmpCodeRegExp_Msg)]
        [MaxLength(ValidationConstants.EmpCode_MaxLength, ErrorMessage = ValidationConstants.EmpCode_MaxLength_Msg)]
        public string EmployeeCode { get; set; }

        //[Required(ErrorMessage = ValidationConstants.EmailIdRequied_Msg)]
        //[RegularExpression(ValidationConstants.EmailRegExp, ErrorMessage = ValidationConstants.EmailRegExp_Msg)]
        [MaxLength(ValidationConstants.Email_MaxLength, ErrorMessage = ValidationConstants.Email_MaxLength_Msg)]
        public string EmailId { get; set; }

        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string MobileNumber { get; set; }

        //[Range(1, long.MaxValue, ErrorMessage = ValidationConstants.RoleRequied_Dropdown_Msg)]
        public long RoleId { get; set; }

        //[Range(1, long.MaxValue, ErrorMessage = ValidationConstants.ReportingToRequied_Dropdown_Msg)]
        public long? ReportingTo { get; set; }

        //[Required(ErrorMessage = ValidationConstants.DateOfBirthRequied_Msg)]
        public DateTime? DateOfBirth { get; set; }

        //[Required(ErrorMessage = ValidationConstants.DateOfJoiningRequied_Msg)]
        public DateTime? DateOfJoining { get; set; }
        public string EmergencyContactNumber { get; set; }
        public int? BloodGroupId { get; set; }
        public bool IsWebUser { get; set; }
        public bool IsMobileUser { get; set; }
        public bool IsActive { get; set; }

        //[Required(ErrorMessage = "Initial Password value is required")]
        //[MaxLength(20, ErrorMessage = "More than 20 characters are not allowed for Initial Password")]
        public string InitialPassword { get; set; }

        [MaxLength(ValidationConstants.MobileUniqueId_MaxLength, ErrorMessage = ValidationConstants.MobileUniqueId_MaxLength_Msg)]
        public string MobileUniqueId { get; set; }

        public string Department { get; set; }
        public int? MaterialStatus  { get; set; }
        public int? GenderId { get; set; }
        public string CompanyNumber { get; set; }
        public string PermanentAddress { get; set; }
        public int? PermanentStateId { get; set; }
        public int? PermanentRegionId { get; set; }
        public int? PermanentDistrictId { get; set; }
        public int? PermanentCityId { get; set; }
        public int? PermanentAreaId { get; set; }
        public decimal? PermanentPinCode { get; set; }
        public bool? IsTemporaryAddressIsSame { get; set; }
        public string? TemporaryAddress { get; set; }
        public int? TemporaryStateId { get; set; }
        public int? TemporaryRegionId { get; set; }
        public int? TemporaryDistrictId { get; set; }
        public int? TemporaryCityId { get; set; }
        public int? TemporaryAreaId { get; set; }
        public decimal? TemporaryPinCode { get; set; }
        public string EmergencyName { get; set; }
        public decimal? EmergencyNumber { get; set; }
        public string EmergencyRelation { get; set; }
        public string EmployeePostCompanyName { get; set; }
        public string TotalNumberOfExp { get; set; }
        public string AddharNumber { get; set; }
        public string PANNumber { get; set; }
        public string OtherProof { get; set; }
        public string Remark { get; set; }

        public IFormFile ProfilePicture { get; set; }
        [JsonIgnore]
        public string FileOriginalName { get; set; }
        [JsonIgnore]
        public string ImageUpload { get; set; }

        public IFormFile AdharCard { get; set; }
        [JsonIgnore]
        public string AdharCardFileName { get; set; }
        [JsonIgnore]
        public string AdharCardSavedFileName { get; set; }

        public IFormFile PanCard { get; set; }
        [JsonIgnore]
        public string PanCardFileName { get; set; }
        [JsonIgnore]
        public string PanCardSavedFileName { get; set; }

        public IFormFile OtherProofAttach { get; set; }
        [JsonIgnore]
        public string OtherProofFileName { get; set; }
        [JsonIgnore]
        public string UploadOtherProof { get; set; }

        public bool IsToDeleteProfilePic { get; set; }
        public bool IsToDeleteAdharCard { get; set; }
        public bool IsToDeletePanCard { get; set; }
        public bool IsToDeleteOtherProofAttach { get; set; }
    }

    public class EmployeeResponse : CreationDetails
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string Passwords { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long ReportingTo { get; set; }
        public string ReportingToName { get; set; }
        public string ManagerMobileNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string EmergencyContactNumber { get; set; }
        public int? BloodGroupId { get; set; }
        public string BloodGroup { get; set; }
        public bool IsWebUser { get; set; }
        public bool IsMobileUser { get; set; }
        public bool IsActive { get; set; }
        public string MobileUniqueId { get; set; }
        //public IFormFile FileOriginalName { get; set; }

        public string Department { get; set; }
        public int? MaterialStatus { get; set; }
        public int? GenderId { get; set; }
        public string Gender { get; set; }
        public string CompanyNumber { get; set; }
        public string PermanentAddress { get; set; }
        public int? PermanentStateId { get; set; }
        public string PermanentStateName { get; set; }

        public int? PermanentRegionId { get; set; }
        public string PermanentRegionName { get; set; }

        public int? PermanentDistrictId { get; set; }
        public string PermanentDistrictName { get; set; }

        public int? PermanentCityId { get; set; }
        public string PermanentCityName { get; set; }

        public int? PermanentAreaId { get; set; }
        public string PermanentAreaName { get; set; }

        public decimal? PermanentPinCode { get; set; }
        public bool? IsTemporaryAddressIsSame { get; set; }
        public string? TemporaryAddress { get; set; }
        public int? TemporaryStateId { get; set; }
        public string TemporaryStateName { get; set; }

        public int? TemporaryRegionId { get; set; }
        public string TemporaryRegionName { get; set; }

        public int? TemporaryDistrictId { get; set; }
        public string TemporaryDistrictName { get; set; }

        public int? TemporaryCityId { get; set; }
        public string TemporaryCityName { get; set; }

        public int? TemporaryAreaId { get; set; }
        public string TemporaryAreaName { get; set; }

        public decimal? TemporaryPinCode { get; set; }
        public string EmergencyName { get; set; }
        public decimal? EmergencyNumber { get; set; }
        public string EmergencyRelation { get; set; }
        public string EmployeePostCompanyName { get; set; }
        public string TotalNumberOfExp { get; set; }
        public string AddharNumber { get; set; }
        public string PANNumber { get; set; }
        public string OtherProof { get; set; }
        public string Remark { get; set; }

        [JsonIgnore]
        public string ImageUpload { get; set; }
        public string FileOriginalName { get; set; }

        public string AdharCardFileName { get; set; }
        public string AdharCardSavedFileName { get; set; }

        public string PanCardFileName { get; set; }
        public string PanCardSavedFileName { get; set; }

        public string OtherProofFileName { get; set; }
        public string UploadOtherProof { get; set; }

        public byte[] ProfilePicture { get; set; }
        public byte[] AdharCardPicture { get; set; }
        public byte[] PanCardPicture { get; set; }
        public byte[] OtherProofPicture { get; set; }

        public string ProfilePictureUrl { get; set; }
        public string AdharCardPictureUrl { get; set; }
        public string PanCardPictureUrl { get; set; }
        public string OtherProofPictureUrl { get; set; }
    }
    public class EmployeeReportingToResponse
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

        public class ProfilePictureRequest
    {
        //[RegularExpression(ValidationConstants.ImageFileRegExp, ErrorMessage = ValidationConstants.ImageFileRegExp_Msg)]
        public IFormFile ProfilePicture { get; set; }
    }

    public class UpdateEmployeeDetailsRequest
    {
        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.EmpIdRequired_Msg)]
        public long EmployeeId { get; set; }

        [Required(ErrorMessage = ValidationConstants.EmployeeNameRequied_Msg)]
        [RegularExpression(ValidationConstants.EmployeeNameRegExp, ErrorMessage = ValidationConstants.EmployeeNameRegExp_Msg)]
        [MaxLength(ValidationConstants.EmployeeName_MaxLength, ErrorMessage = ValidationConstants.EmployeeName_MaxLength_Msg)]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = ValidationConstants.EmpCodeRequired_Msg)]
        [RegularExpression(ValidationConstants.EmpCodeRegExp, ErrorMessage = ValidationConstants.EmpCodeRegExp_Msg)]
        [MaxLength(ValidationConstants.EmpCode_MaxLength, ErrorMessage = ValidationConstants.EmpCode_MaxLength_Msg)]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = ValidationConstants.EmailIdRequied_Msg)]
        [RegularExpression(ValidationConstants.EmailRegExp, ErrorMessage = ValidationConstants.EmailRegExp_Msg)]
        [MaxLength(ValidationConstants.Email_MaxLength, ErrorMessage = ValidationConstants.Email_MaxLength_Msg)]
        public string EmailId { get; set; }

        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string MobileNumber { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.RoleRequied_Dropdown_Msg)]
        public long RoleId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.ReportingToRequied_Dropdown_Msg)]
        public long ReportingTo { get; set; }

        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string ManagerMobileNumber { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.AddressIdRequired_Msg)]
        public long AddressId { get; set; }

        [Required(ErrorMessage = ValidationConstants.AddressRequied_Msg)]
        [MaxLength(ValidationConstants.Address_MaxLength, ErrorMessage = ValidationConstants.Address_MaxLength_Msg)]
        public string Address { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.StateRequied_Dropdown_Msg)]
        public long StateId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.ReportingToRequied_Dropdown_Msg)]
        public long RegionId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.DistrictRequied_Dropdown_Msg)]
        public long DistrictId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = ValidationConstants.AreaRequied_Dropdown_Msg)]
        public long AreaId { get; set; }

        [Required(ErrorMessage = ValidationConstants.PincodeRequied_Msg)]
        [RegularExpression(ValidationConstants.PincodeExp, ErrorMessage = ValidationConstants.Pincode_Validation_Msg)]
        [MaxLength(ValidationConstants.Pincode_MaxLength, ErrorMessage = ValidationConstants.Pincode_MaxLength_Msg)]
        [MinLength(ValidationConstants.Pincode_MinLength, ErrorMessage = ValidationConstants.Pincode_MinLength_Msg)]
        public string Pincode { get; set; }
    }

    public class ImportedEmployeeDetails
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string RoleName { get; set; }
        public string ReportingToName { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public string AreaName { get; set; }
        public string Pincode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string BloodGroup { get; set; }
        public string IsWebUser { get; set; }
        public string IsMobileUser { get; set; }
        public string IsActive { get; set; }
    }

    public class EmployeeDataValidationErrors
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmailId { get; set; }
        public string MobileNumber { get; set; }
        public string RoleName { get; set; }
        public string ReportingToName { get; set; }
        public string Address { get; set; }
        public string StateName { get; set; }
        public string RegionName { get; set; }
        public string DistrictName { get; set; }
        public string AreaName { get; set; }
        public string Pincode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string BloodGroup { get; set; }
        public string IsWebUser { get; set; }
        public string IsMobileUser { get; set; }
        public string ValidationMessage { get; set; }
        public string IsActive { get; set; }
    }

    public class SearchModuleMasterRequest
    {
        public PaginationParameters pagination { get; set; }
        public string ModuleName { get; set; }
        public string AppType { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }

    public class ModuleMaster_Request
    {
        public long ModuleId { get; set; }
        public long ModuleName { get; set; }
        public long AppType { get; set; }
        public long IsActive { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class ModuleMaster_Response
    {
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string AppType { get; set; }
        public bool IsActive { get; set; }

        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }

    public class ModuleList
    {
        public long ModuleId { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
    }

    public class SearchRoleMaster_PermissionRequest
    {
        public PaginationParameters pagination { get; set; }
        public long RoleId { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }

    public class RoleMaster_Permission_Request
    {
        public long RolePermissionId { get; set; }
        public long RoleId { get; set; }
        public string AppType { get; set; }
        public bool IsActive { get; set; }

        public List<ModuleList> ModuleList { get; set; }
    }

    public class RoleMaster_Permission_Response
    {
        public string AppType { get; set; }
        //public long RolePermissionId { get; set; }
        //public long RoleId { get; set; }
        //public string RoleName { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
    }

    public class SearchRoleMaster_Employee_PermissionRequest
    {
        public PaginationParameters pagination { get; set; }
        public long EmployeeId { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }

    public class RoleMaster_Employee_Permission_Request
    {
        public long RolePermissionId { get; set; }
        public long RoleId { get; set; }
        public string AppType { get; set; }
        public long EmployeeId { get; set; }
        public bool IsActive { get; set; }

        public List<ModuleList> ModuleList { get; set; }
    }

    public class RoleMaster_Employee_Permission_Response
    {
        public string AppType { get; set; }
        //public long EmployeePermissionId { get; set; }
        //public long EmployeeId { get; set; }
        //public long RoleId { get; set; }
        //public string RoleName { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
    }
}
