using CasaAPI.Models;

namespace CasaAPI.Interfaces.Services
{
    public interface IJwtUtilsService
    {
        public (string, DateTime) GenerateJwtToken(UsersLoginSessionData parameters);
        Task<UsersLoginSessionData?> ValidateJwtToken(string token);
    }
}
