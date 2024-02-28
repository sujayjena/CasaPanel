using System.Text.Json.Serialization;

namespace CasaAPI.Models
{
    public class UsersLoginSessionData
    {
        public long UserId { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        public short LoginRetryAttempt { get; set; }
        public bool IsUserLocked { get; set; }
        public bool IsActive { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        [JsonIgnore]
        public bool IsCorrectPassword { get; set; }                
        public string EmployeeCode { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        //public string CompanyName { get; set; }
        //public int? CustomerTypeId { get; set; }
        //public string CustomerTypeName { get; set; }

    }

    public class SessionDataCustomer
    {
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        public string Name { get; set; }
        public string CustomerTypeName { get; set; }
        public string Token { get; set; }
    }

    public class SessionDataEmployee
    {
        public int? EmployeeId { get; set; }
        //public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int? RoleId { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }
        public string RoleName { get; set; }
        public string Token { get; set; }
        public string EmployeeCode { get; set; }
    }
}
