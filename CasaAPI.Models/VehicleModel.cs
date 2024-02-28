using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class VehicleModel
    {
        public class VehicleSaveParameters
        {
            public int VehicleId { get; set; }
            [Required(ErrorMessage = ValidationConstants.VehicleNumberRequied_Msg)]
            [RegularExpression(ValidationConstants.VehicleNumberRegExp, ErrorMessage = ValidationConstants.VehicleNumberRegExp_Msg)]
            [MaxLength(ValidationConstants.VehicleNumber_MaxLength, ErrorMessage = ValidationConstants.VehicleNumber_MaxLength_Msg)]
            public string VehicleNumber { get; set; }
            public bool IsActive { get; set; }
        }
        public class VehicleDetailsResponse : LogParameters
        {
            public int VehicleId { get; set; }
            public string VehicleNumber { get; set; }

        }
        public class VehicleSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }
            public bool? IsExport { get; set; }

        }
        public class VehicleImportSaveParameters
        {
            [Required(ErrorMessage = ValidationConstants.VehicleNumberRequied_Msg)]
            [RegularExpression(ValidationConstants.VehicleNumberRegExp, ErrorMessage = ValidationConstants.VehicleNumberRegExp_Msg)]
            [MaxLength(ValidationConstants.VehicleNumber_MaxLength, ErrorMessage = ValidationConstants.VehicleNumber_MaxLength_Msg)]
            public string VehicleNumber { get; set; }
            [Required(ErrorMessage = ValidationConstants.IsActiveYesNoRequired_Msg)]
            [RegularExpression(ValidationConstants.IsActiveYesNoRegExp, ErrorMessage = ValidationConstants.IsActiveYesNoRegExp_Msg)]
            public string IsActive { get; set; }
        }
        public class VehicleFailToImportValidationErrors
        {
            public string VehicleNumber { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
