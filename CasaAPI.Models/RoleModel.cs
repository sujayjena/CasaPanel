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
    public class RoleSaveParameters
    {
        public int RoleId { get; set; }
        //[Required(ErrorMessage = ValidationConstants.RoleNameRequied_Msg)]
        //[RegularExpression(ValidationConstants.RoleNameRegExp, ErrorMessage = ValidationConstants.RoleNameRegExp)]
        //[MaxLength(ValidationConstants.RoleName_MaxLength, ErrorMessage = ValidationConstants.RoleName_MaxLength_Msg)]
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }
    public class RoleDetailsResponse : LogParameters
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }
    public class RoleSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
    public class RoleImportSaveParameters
    {
        [Required(ErrorMessage = ValidationConstants.RoleNameRequied_Msg)]
        [RegularExpression(ValidationConstants.RoleNameRegExp, ErrorMessage = ValidationConstants.RoleNameRegExp_Msg)]
        [MaxLength(ValidationConstants.RoleName_MaxLength, ErrorMessage = ValidationConstants.RoleName_MaxLength_Msg)]
        public string RoleName { get; set; }
        [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
        [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
        public string IsActive { get; set; }
    }
    public class RoleFailToImportValidationErrors
    {
        public string RoleName { get; set; }
        public string IsActive { get; set; }
        public string ValidationMessage { get; set; }
    }
}
