using CasaAPI.Models;

namespace CasaAPI.Interfaces.Services
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveResponse>> GetLeavesList(SearchLeaveRequest request);
        Task<int> SaveLeaveDetails(LeaveRequest leaveRequest);
        Task<int> UpdateLeaveStatus(UpdateLeaveStatusRequest parameters);
        Task<LeaveResponse?> GetLeaveDetailsById(long id);
    }
}
