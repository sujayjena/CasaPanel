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
    public class CalanderModel
    {
        public class CalanderSaveParameters
        {
            public int CalanderId { get; set; }
            public string? CalanderName { get; set; }
            public bool IsActive { get; set; }
        }
        public class CalanderDetailsResponse : LogParameters
        {
            public int CalanderId { get; set; }
            public string CalanderName { get; set; }

        }
        public class CalanderSearchParameters
        {
            public PaginationParameters pagination { get; set; }
            public string ValueForSearch { get; set; } = null;
            public bool? IsActive { get; set; }

            [JsonIgnore]
            public bool? IsExport { get; set; }

        }
        public class CalanderImportSaveParameters
        {
            public string CalanderName { get; set; }
            public string IsActive { get; set; }
        }
        public class CalanderFailToImportValidationErrors
        {
            public string CalanderName { get; set; }
            public string IsActive { get; set; }
            public string ValidationMessage { get; set; }
        }
    }
}
