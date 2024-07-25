using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class MaritalStatusModel
    {
    }

    public class MaritalStatus_Request
    {
        public int Id { get; set; }
        [DefaultValue("")]
        public string? MaritalStatusName { get; set; }

        public bool? IsActive { get; set; }
    }
    public class SearchMaritalStatus_Request
    {
        public PaginationParameters pagination { get; set; }

        [DefaultValue("")]
        public string ValueForSearch { get; set; } = null;
        public bool? IsActive { get; set; }
    }
    public class MaritalStatus_Response : CreationDetails
    {
        public int Id { get; set; }
        public string? MaritalStatusName { get; set; }

        public bool? IsActive { get; set; }
    }
}
