using System.ComponentModel;

namespace CasaAPI.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        [DefaultValue(0)]
        public long Total { get; set; }
        public object Data { get; set; }
    }
}
