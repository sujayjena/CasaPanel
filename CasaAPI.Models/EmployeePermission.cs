using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class EmployeePermissionUpdateParameters
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int RoleId { get; set; }
        public int EmployeeId { get; set; }
        public bool ViewData { get; set; }
        public bool AddData { get; set; }
        public bool EditData { get; set; }

        public bool IsActive { get; set; }
    }
    public class EmployeePermissionDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string PageName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public bool ViewData { get; set; }
        public bool AddData { get; set; }
        public bool EditData { get; set; }
    }

    public class EmployeePermissionSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public int EmployeeId { get; set; } = 0;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
}
