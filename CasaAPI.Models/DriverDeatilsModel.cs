using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            public string DriverName { get; set; }
            public string VehicleNumber { get; set; }
            public string MobileNumber { get; set; }

            [DefaultValue("")]
            public string ProfileFileName { get; set; }

            [DefaultValue("")]
            public string ProfileSavedFileName { get; set; }

            [DefaultValue("")]
            public string ProfileSavedFileName_Base64 { get; set; }
            public bool IsActive { get; set; }
        }
        public class DriverDetailsResponse : LogParameters
        {
            public int DriverId { get; set; }
            public string DriverName { get; set; }
            public string VehicleNumber { get; set; }
            public string MobileNumber { get; set; }
            public string ProfileFileName { get; set; }
            public string ProfileSavedFileName { get; set; }
            public string ProfileFileURL { get; set; }
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
            public string DriverName { get; set; }
            public string VehicleNumber { get; set; }
            public string MobileNumber { get; set; }
            public string IsActive { get; set; }
        }
        public class DriverFailToImportValidationErrors
        {
            public string DriverName { get; set; }
            public string VehicleNumber { get; set; }
            public string MobileNumber { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
