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

}
