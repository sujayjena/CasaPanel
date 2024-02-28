using Microsoft.AspNetCore.Http;

namespace CasaAPI.Models
{
    public class CreationDetails
    {
        public string CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int TotalRecords { get; set; }
    }
    public class LogParameters : CreationDetails
    {
        public dynamic IsActive { get; set; }
        //public bool IsDeleted { get; set; }
        //public long? ModifiedBy { get; set; }
        //public DateTime? ModifiedOn { get; set; }
    }
    public class ImportRequest
    {
        public IFormFile FileUpload { get; set; }
    }
}
