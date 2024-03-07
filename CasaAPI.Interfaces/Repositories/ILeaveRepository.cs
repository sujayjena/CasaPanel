using CasaAPI.Models;

namespace CasaAPI.Interfaces.Repositories
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<LeaveResponse>> GetLeavesList(SearchLeaveRequest request);
        Task<int> SaveLeaveDetails(LeaveRequest parameters);
        Task<int> UpdateLeaveStatus(UpdateLeaveStatusRequest parameters);
        Task<LeaveResponse?> GetLeaveDetailsById(long id);
        
    }
}
