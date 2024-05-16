﻿using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<VisitCustomerCountListResponse>> GetVisitCustomerCountList(SearchVisitCountListRequest request);
        Task<IEnumerable<DayWiseVisitCountListResponse>> GetDayWiseVisitCountList(SearchVisitCountListRequest request);
    }
}
