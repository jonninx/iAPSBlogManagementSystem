using API.Interfaces;
using API.Models.ResponseModels;
using AutoMapper;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;
        public NotificationService(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> CreateNotificationAsync(Notification notification)
        {
            try
            {
                _context.Notifications.Add(notification);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            try
            {
                return await _context.Notifications
                                 .Where(n => n.UserId == userId)
                                 .OrderByDescending(n => n.CreatedAt)
                                 .Select(n => new NotificationDto
                                 {
                                     NotificationId = n.NotificationId,
                                     Content = n.Content,
                                     CreatedAt = n.CreatedAt,
                                     IsRead = n.IsRead
                                 })
                    .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> MarkNotificationAsReadAsync(Guid notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification == null) return false;

                notification.IsRead = true;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
