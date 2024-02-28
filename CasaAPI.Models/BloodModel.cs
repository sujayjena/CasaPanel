using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class BloodModel
    {
        public class BloodSaveParameters
        {
            public int BloodId { get; set; }
             public string BloodGroup { get; set; }
            public bool IsActive { get; set; }
        }
        public class BloodDetailsResponse : LogParameters
        {
            public int BloodId { get; set; }
            public string BloodGroup { get; set; }

        }
        public class BloodSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class BloodImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.BloodGroupRequied_Msg)]
            [RegularExpression(ValidationConstants.BloodGroupRegExp, ErrorMessage = ValidationConstants.BloodGroupRegExp_Msg)]
            [MaxLength(ValidationConstants.BloodGroup_MaxLength, ErrorMessage = ValidationConstants.BloodGroup_MaxLength_Msg)]
            public string BloodGroup { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class BloodFailToImportValidationErrors
        {
            public string BloodGroup { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
