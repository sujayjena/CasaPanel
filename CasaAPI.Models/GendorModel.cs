using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class GendorModel
    {
        public class GendorSaveParameters
        {
            public int GenderId { get; set; }
            [Required(ErrorMessage = ValidationConstants.GendorRequied_Msg)]
            [RegularExpression(ValidationConstants.GendorRegExp, ErrorMessage = ValidationConstants.GendorRegExp_Msg)]
            [MaxLength(ValidationConstants.Gender_MaxLength, ErrorMessage = ValidationConstants.Gendor_MaxLength_Msg)]
            public string Gender { get; set; }
            public bool IsActive { get; set; }
        }
        public class GendorDetailsResponse : LogParameters
        {
            public int GenderId { get; set; }
            public string Gender { get; set; }

        }
        public class GendorSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class GendorImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.GendorRequied_Msg)]
            [RegularExpression(ValidationConstants.GendorRegExp, ErrorMessage = ValidationConstants.GendorRegExp_Msg)]
            [MaxLength(ValidationConstants.Gender_MaxLength, ErrorMessage = ValidationConstants.Gendor_MaxLength_Msg)]
            public string Gender { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class GendorFailToImportValidationErrors
        {
            public string Gender { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
