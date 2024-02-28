using CasaAPI.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace CasaAPI.Models
{
    public class LoginByEmailRequestModel
    {
        //[Required(ErrorMessage = ValidationConstants.EmailIdRequied_Msg)]
        //[RegularExpression(ValidationConstants.EmailRegExp, ErrorMessage = ValidationConstants.EmailRegExp_Msg)]
        //[MaxLength(100, ErrorMessage = ValidationConstants.Email_MaxLength_Msg)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }

    public class LoginOTPRequestModel
    {
        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string MobileNo { get; set; }
    }

    public class LoginByOTPRequestModel : LoginOTPRequestModel
    {
        [Required(ErrorMessage = ValidationConstants.OTP_Required_Msg)]
        [RegularExpression(ValidationConstants.OTP_RegExp, ErrorMessage = ValidationConstants.OTP_RegExp_Msg)]
        [MinLength(ValidationConstants.OTP_MinLength, ErrorMessage = ValidationConstants.OTP_Range_Msg)]
        [MaxLength(ValidationConstants.OTP_MaxLength, ErrorMessage = ValidationConstants.OTP_Range_Msg)]
        public string OTP { get; set; }
    }
}
