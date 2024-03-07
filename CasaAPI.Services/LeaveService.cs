using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;

namespace CasaAPI.Services
{
    public class LeaveService: ILeaveService
    {
        private ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public async Task<IEnumerable<LeaveResponse>> GetLeavesList(SearchLeaveRequest request)
        {
            return await _leaveRepository.GetLeavesList(request);
        }

        public async Task<int> SaveLeaveDetails(LeaveRequest leaveRequest)
        {
            return await _leaveRepository.SaveLeaveDetails(leaveRequest);
        }

        public async Task<int> UpdateLeaveStatus(UpdateLeaveStatusRequest parameters)
        {
            return await _leaveRepository.UpdateLeaveStatus(parameters);
        }

        public async Task<LeaveResponse?> GetLeaveDetailsById(long id)
        {
            return await _leaveRepository.GetLeaveDetailsById(id);
        }
    }
}
