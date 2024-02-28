using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class DriverDeatilsModel
    {
        public class DriverSaveParameters
        {
            public int DriverId { get; set; }
            [Required(ErrorMessage = ValidationConstants.DriverNameRequied_Msg)]
            [RegularExpression(ValidationConstants.DriverNameRegExp, ErrorMessage = ValidationConstants.DriverNameRegExp_Msg)]
            [MaxLength(ValidationConstants.DriverName_MaxLength, ErrorMessage = ValidationConstants.DriverName_MaxLength_Msg)]
            public string DriverName { get; set; }
            public string VehicleNumber { get; set; }
            public string ProfilePath { get; set; }

            [Required]
            [RegularExpression(@"^([0]|\+91[\-\s]?)?[789]\d{9}$", ErrorMessage = "Entered Mobile No is not valid.")]
            public string MobileNumber { get; set; }
            public bool IsActive { get; set; }
        }
        public class DriverDetailsResponse : LogParameters
        {
            public int DriverId { get; set; }
            public string DriverName { get; set; }
            public string VehicleNumber { get; set; }
            public string ProfilePath { get; set; }

         
            public string MobileNumber { get; set; }

        }
        public class DriverSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class DriverImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.DriverNameRequied_Msg)]
            [RegularExpression(ValidationConstants.DriverNameRegExp, ErrorMessage = ValidationConstants.DriverNameRegExp_Msg)]
            [MaxLength(ValidationConstants.DriverName_MaxLength, ErrorMessage = ValidationConstants.DriverName_MaxLength_Msg)]
            public string DriverName { get; set; }
            public string VehicleNumber { get; set; }
            public string ProfilePath { get; set; }

            [Required]
            [RegularExpression(@"^([0]|\+91[\-\s]?)?[789]\d{9}$", ErrorMessage = "Entered Mobile No is not valid.")]
            public string MobileNumber { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class DriverFailToImportValidationErrors
        {
            public string DriverName { get; set; }
            public string VehicleNumber { get; set; }
            public string ProfilePath { get; set; }
            public string MobileNumber { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
