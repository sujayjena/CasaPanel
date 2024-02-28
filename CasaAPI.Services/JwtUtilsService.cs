using CasaAPI.Helpers;
using CasaAPI.Models;
using CasaAPI.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace CasaAPI.Services
{
    public class JwtUtilsService : IJwtUtilsService
    {
        private readonly AppSettings _appSettings;
        private readonly IProfileService _profileService;

        public JwtUtilsService(IOptions<AppSettings> appSettings, IProfileService profileService)
        {
            _appSettings = appSettings.Value;
            _profileService = profileService;
        }

        public (string, DateTime) GenerateJwtToken(UsersLoginSessionData parameters)
        {
            string token;
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.JWT.SecretKey);
            List<Claim> claims = new List<Claim>();
            DateTime tokenExpiryDateTime;

            claims.Add(new Claim("Id", EncryptDecryptHelper.EncryptString(parameters.UserId.ToString())));
            if (parameters.EmailAddress != null)
            {
                claims.Add(new Claim("EmailAddress", parameters.EmailAddress));
            }
            claims.Add(new Claim("MobileNo", parameters.MobileNo));

            if (parameters.EmployeeId != null)
            {
                //claims.Add(new Claim("EmployeeCode", parameters.EmployeeCode));
                //claims.Add(new Claim("Name", parameters.EmployeeName));
                //claims.Add(new Claim("RoleName", parameters.RoleName));
            }
            //else if (parameters.CustomerId != null)
            //{
            //    //claims.Add(new Claim("Name", parameters.CompanyName));
            //    //claims.Add(new Claim("CustomerTypeName", parameters.CustomerTypeName));
            //}

            tokenExpiryDateTime = DateTime.Now.AddMinutes(60);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiryDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            token = tokenHandler.WriteToken(securityToken);

            return (token, tokenExpiryDateTime);
        }

        public async Task<UsersLoginSessionData?> ValidateJwtToken(string token)
        {
            byte[] key;
            JwtSecurityTokenHandler tokenHandler;
            JwtSecurityToken jwtToken;
            UsersLoginSessionData? response=null;

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            try
            {
                tokenHandler = new JwtSecurityTokenHandler();
                key = Encoding.ASCII.GetBytes(_appSettings.JWT.SecretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtToken = (JwtSecurityToken)validatedToken;

                //LastAccessOn in UsersLoginHistory table is updating here from SP "GetProfileDetailsByToken"
                response = await _profileService.GetProfileDetailsByToken(token);

                return response;
            }
            catch
            {
                return null;
            }
        }
    }
}
