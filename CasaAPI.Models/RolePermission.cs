using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class PageSaveParameters
    {
        public int PageId { get; set; }
        public string PageName { get; set; }

        public int RoleId { get; set; } = 0;

        public int EmployeeId { get; set; } = 0;
        public bool IsActive { get; set; }
    }
    public class PageDetailsResponse : LogParameters
    {
        public int PageId { get; set; }
        public string PageName { get; set; }

    }
    public class PageSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }

    public class RolePermissionUpdateParameters
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public int RoleId { get; set; }

        public bool ViewData { get; set; }
        public bool AddData { get; set; }
        public bool EditData { get; set; }

        public bool IsActive { get; set; }
    }
    public class RollPermissionDetailsResponse : LogParameters
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string PageName { get; set; }
        public int RoleId {  get; set; }        
        public string RoleName { get; set; }
        public bool ViewData { get; set; }
        public bool AddData { get; set; }
        public bool EditData { get; set; }
    }

    public class RollPermissionSearchParameters
    {
        public PaginationParameters pagination { get; set; }
        public string ValueForSearch { get; set; } = null;
        public int RoleId { get; set; }= 0;
        public bool? IsActive { get; set; }

        [JsonIgnore]
        public bool? IsExport { get; set; }
    }
    public class RollPermissionListResponse
    {
        public int PageId { get; set;}
        public string PageName { get; set; }

        public List<RollAccessList> rollAccessLists { get; set; }
    }
    public class RollAccessList
    {
        public int RoleId { get; set;}

        public string RoleName { get; set; }

        public bool Access { get; set;}
    }
}
