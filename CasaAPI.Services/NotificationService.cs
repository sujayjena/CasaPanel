using CasaAPI.Interfaces.Repositories;
using CasaAPI.Interfaces.Services;
using CasaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Services
{
    public class NotificationService : INotificationService
    {
        private INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public async Task<IEnumerable<NotificationResponse>> GetNotificationList(SearchNotificationRequest request)
        {
            return await _notificationRepository.GetNotificationList(request);
        }
        public async Task<IEnumerable<NotificationResponse>> GetNotificationListById(long employeeId)
        {
            return await _notificationRepository.GetNotificationListById(employeeId);
        }
    }
}
