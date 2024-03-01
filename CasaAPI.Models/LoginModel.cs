﻿using CasaAPI.Models.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CasaAPI.Models
{
    public class LoginByMobileNoRequestModel
    {
        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [JsonIgnore]
        public string? MobileUniqueId { get; set; }

        [DefaultValue("W")]
        public string IsWebOrMobileUser { get; set; }

        public bool Remember { get; set; }
    }

    public class MobileAppLoginRequestModel
    {
        [Required(ErrorMessage = ValidationConstants.MobileNumberRequied_Msg)]
        [RegularExpression(ValidationConstants.MobileNumberRegExp, ErrorMessage = ValidationConstants.MobileNumberRegExp_Msg)]
        [MaxLength(ValidationConstants.MobileNumber_MaxLength, ErrorMessage = ValidationConstants.MobileNumber_MaxLength_Msg)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mobile Unique Id is required")]
        [MaxLength(ValidationConstants.MobileUniqueId_MaxLength, ErrorMessage = ValidationConstants.MobileUniqueId_MaxLength_Msg)]
        public string MobileUniqueId { get; set; }

        [DefaultValue("M")]
        public string IsWebOrMobileUser { get; set; }
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
