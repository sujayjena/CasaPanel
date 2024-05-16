using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<IEnumerable<VisitCustomerCountListResponse>> GetVisitCustomerCountList(SearchVisitCountListRequest request);
        Task<IEnumerable<DayWiseVisitCountListResponse>> GetDayWiseVisitCountList(SearchVisitCountListRequest request);
    }
}
