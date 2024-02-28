using CasaAPI.Models;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        Task<UsersLoginSessionData?> ValidateUserLoginByUsername(LoginByEmailRequestModel parameters);
        Task SaveUserLoginHistory(UserLoginHistorySaveParameters parameters);
        Task<UsersLoginSessionData?> GetProfileDetailsByToken(string token);
    }
}
