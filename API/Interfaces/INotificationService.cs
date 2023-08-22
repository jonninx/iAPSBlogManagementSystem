using API.Models.ResponseModels;
using Core.Entities;

namespace API.Interfaces
{
    public interface INotificationService
    {
        Task<bool> CreateNotificationAsync(Notification notification);
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId);
        Task<bool> MarkNotificationAsReadAsync(Guid notificationId);
    }
}
