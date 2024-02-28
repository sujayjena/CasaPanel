using CasaAPI.Helpers;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models.Constants;
using CasaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using CasaAPI.CustomAttributes;

namespace CasaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ResponseModel _response;
        private IProfileService _profileService;
        private IJwtUtilsService _jwt;

        public LoginController(IProfileService profileService, IJwtUtilsService jwt)
        {
            _profileService = profileService;
            _jwt = jwt;

            _response = new ResponseModel();
            _response.IsSuccess = true;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ResponseModel> Login(LoginByEmailRequestModel parameters)
        {
            (string, DateTime) tokenResponse;
            SessionDataEmployee employeeSessionData;
            //SessionDataCustomer customerSessionData;
            UsersLoginSessionData? loginResponse=null;
            UserLoginHistorySaveParameters loginHistoryParameters;

            loginResponse = await _profileService.ValidateUserLoginByUsername(parameters);
            
            _response.IsSuccess = false;

            if (loginResponse == null)
            {
                _response.Message = ErrorConstants.UserNotExistsError;
            }
            else if (loginResponse != null)
            {
                if (loginResponse.IsCorrectPassword == false)
                {
                    _response.Data = new
                    {
                        LoginRetryAttempt = loginResponse?.LoginRetryAttempt,
                        IsUserLocked = loginResponse?.IsUserLocked
                    };

                    _response.Message = ErrorConstants.InvalidCredentialsError;
                }
                else if (loginResponse.IsActive == false)
                {
                    _response.Message = ErrorConstants.InactiveProfileError;
                }
                else if (loginResponse.IsUserLocked == true)
                {
                    _response.Message = ErrorConstants.LockedProfileError;
                }
                else
                {
                    tokenResponse = _jwt.GenerateJwtToken(loginResponse);

                    if (loginResponse.EmployeeId != null)
                    {
                        employeeSessionData = new SessionDataEmployee
                        {
                            EmployeeId = loginResponse.EmployeeId,
                            EmailAddress = loginResponse.EmailAddress,
                            MobileNo = loginResponse.MobileNo,
                            EmployeeName = loginResponse.EmployeeName,
                            EmployeeCode = loginResponse.EmployeeCode,
                            RoleId = loginResponse.RoleId,
                            RoleName = loginResponse.RoleName,
                            Token = tokenResponse.Item1
                        };

                        _response.Data = employeeSessionData;
                    }
                    //else if (loginResponse.CustomerId != null)
                    //{
                    //    customerSessionData = new SessionDataCustomer
                    //    {
                    //        EmailAddress = loginResponse.EmailAddress,
                    //        MobileNo = loginResponse.MobileNo,
                    //        //Name = loginResponse.CompanyName,
                    //        //CustomerTypeName = loginResponse.CustomerTypeName,
                    //        Token = tokenResponse.Item1
                    //    };

                    //    _response.Data = customerSessionData;
                    //}

                    //Login History
                    loginHistoryParameters = new UserLoginHistorySaveParameters
                    {
                        UserId = loginResponse.UserId,
                        UserToken = tokenResponse.Item1,
                        IsLoggedIn = true,
                        IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                        DeviceName = HttpContext.Request.Headers["User-Agent"],
                        TokenExpireOn = tokenResponse.Item2,
                        RememberMe = parameters.Remember
                    };

                    await _profileService.SaveUserLoginHistory(loginHistoryParameters);

                    _response.IsSuccess = true;
                    _response.Message = MessageConstants.LoginSuccessful;
                }
            }

            return _response;
        }

        [HttpPost]
        [Route("[action]")]
        [CustomAuthorize]
        public async Task<ResponseModel> Logout()
        {
            string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last().SanitizeValue()!;
            //UsersLoginSessionData? sessionData = (UsersLoginSessionData?)HttpContext.Items["SessionData"]!;

            UserLoginHistorySaveParameters logoutParameters = new UserLoginHistorySaveParameters
            {
                UserId = SessionManager.LoggedInUserId,
                UserToken = token,
                IsLoggedIn = false, //To Logout set IsLoggedIn = false
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                DeviceName = HttpContext.Request.Headers["User-Agent"],
                TokenExpireOn = DateTime.Now,
                RememberMe = false
            };

            await _profileService.SaveUserLoginHistory(logoutParameters);

            _response.Message = MessageConstants.LogoutSuccessful;

            return _response;
        }
        //[HttpPost]
        //[Route("[action]")]
        //public async Task<ResponseModel> GetOTPForCustomerLogin(LoginOTPRequestModel parameters)
        //{
        //    _response.IsSuccess = true;
        //    _response.Message = MessageConstants.OTPSentSuccessful;

        //    return _response;
        //}

        //[HttpPost]
        //[Route("[action]")]
        //public async Task<ResponseModel> GetOTPForEmployeeLogin(LoginOTPRequestModel parameters)
        //{
        //    _response.IsSuccess = true;
        //    _response.Message = MessageConstants.OTPSentSuccessful;

        //    return _response;
        //}

        //[HttpPost]
        //[Route("[action]")]
        //public async Task<ResponseModel> LoginByMobileOTP(LoginByOTPRequestModel parameters)
        //{
        //    _response.IsSuccess = true;
        //    _response.Message = MessageConstants.LoginSuccessful;

        //    return _response;
        //}
    }
}
