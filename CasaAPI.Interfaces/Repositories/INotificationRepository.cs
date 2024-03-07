using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationResponse>> GetNotificationList(SearchNotificationRequest request);
        Task<IEnumerable<NotificationResponse>> GetNotificationListById(long employeeId);
    }
}
