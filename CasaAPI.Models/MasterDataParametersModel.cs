using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class SelectListResponse
    {
        public long Value { get; set; }
        public string Text { get; set; }
    }

    public class CommonSelectListRequestModel
    {
        public bool? IsActive { get; set; }
    }
    public class CustomerSelectListRequestModel : CommonSelectListRequestModel
    {
        public long? CustomerTypeId { get; set; }
    }

    public class ReportingToEmpListParameters
    {
        public long RoleId { get; set; }
        public long? RegionId { get; set; }
    }
}
