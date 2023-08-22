using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService= notificationService;
        }

        [HttpGet("notifications")]
        [Authorize]
        public async Task<IActionResult> GetUserNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPost("notifications/read/{notificationId}")]
        [Authorize]
        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(notificationId);
            if (result)
            {
                return Ok(new { message = "Notification marked as read." });
            }
            return BadRequest("Failed to mark notification as read.");
        }
    }
}
