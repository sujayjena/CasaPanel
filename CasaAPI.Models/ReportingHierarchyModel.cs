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
    public class ReportingHierarchySaveParameters
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ReportingRoleId { get; set; }
        public bool IsActive { get; set; }
    }
    public class ReportingHierarchyDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string ReportingRoleName { get; set; }
        public string ReportingRoleId { get; set; }

    }
    public class ReportingHierarchySearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
    public class ReportingHierarchyImportSaveParameters
    {
        [Required(ErrorMessage = ValidationConstants.RoleNameRequied_Msg)]
        [RegularExpression(ValidationConstants.RoleNameRegExp, ErrorMessage = ValidationConstants.RoleNameRegExp_Msg)]
        [MaxLength(ValidationConstants.RoleName_MaxLength, ErrorMessage = ValidationConstants.RoleName_MaxLength_Msg)]
        public string RoleName { get; set; }
        [Required(ErrorMessage = ValidationConstants.ReportingRoleNameRequied_Msg)]
        [RegularExpression(ValidationConstants.ReportingRoleNameRegExp, ErrorMessage = ValidationConstants.ReportingRoleNameRegExp_Msg)]
        [MaxLength(ValidationConstants.ReportingRoleName_MaxLength, ErrorMessage = ValidationConstants.ReportingRoleName_MaxLength_Msg)]
        public string ReportingRoleName { get; set; }
        [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
        [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
        public string IsActive { get; set; }
    }
    public class ReportingHierarchyFailToImportValidationErrors
    {
        public string RoleName { get; set; }
        public string ReportingRoleName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
