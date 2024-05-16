using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<IEnumerable<VisitCustomerCountListResponse>> GetVisitCustomerCountList(SearchVisitCountListRequest request)
        {
            return await _dashboardRepository.GetVisitCustomerCountList(request);
        }
        public async Task<IEnumerable<DayWiseVisitCountListResponse>> GetDayWiseVisitCountList(SearchVisitCountListRequest request)
        {
            return await _dashboardRepository.GetDayWiseVisitCountList(request);
        }
    }
}
