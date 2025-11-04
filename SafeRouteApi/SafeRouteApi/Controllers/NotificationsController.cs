using Microsoft.AspNetCore.Mvc;
using SafeRouteApi.Models;

namespace SafeRouteApi.Controllers
{
    /// <summary>
    /// API Controller for managing notifications
    /// Follows the pattern from WeatherForecastController
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]  // Route: api/notifications
    public class NotificationsController : ControllerBase
    {
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(ILogger<NotificationsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: api/notifications
        /// Get all notifications
        /// </summary>
        [HttpGet(Name = "GetAllNotifications")]
        public IActionResult GetAll()
        {
            _logger.LogInformation("Getting all notifications");

            // Mock data - replace with database call
            var notifications = new List<Notifications>
            {
                new Notifications
                {
                    NotificationId = 1,
                    DriverId = 1,
                    Type = "Safety",
                    Title = "High Risk Area Alert: Nyanga",
                    Message = "You are approaching Nyanga, a high-risk area with 15 recent incidents.",
                    Priority = "High",
                    IsRead = false,
                    SentAt = DateTime.Now.AddMinutes(-30)
                },
                new Notifications
                {
                    NotificationId = 2,
                    DriverId = 1,
                    Type = "Weather",
                    Title = "Weather Alert: Khayelitsha",
                    Message = "Heavy rain expected from 3 PM. Road conditions may be hazardous.",
                    Priority = "Medium",
                    IsRead = false,
                    SentAt = DateTime.Now.AddHours(-2)
                },
                new Notifications
                {
                    NotificationId = 3,
                    DriverId = 1,
                    Type = "Traffic",
                    Title = "Peak Hour Traffic Alert",
                    Message = "Heavy traffic expected on major routes.",
                    Priority = "Low",
                    IsRead = true,
                    SentAt = DateTime.Now.AddHours(-4),
                    ReadAt = DateTime.Now.AddHours(-3)
                }
            };

            return Ok(notifications);
        }

        /// <summary>
        /// GET: api/notifications/{id}
        /// Get notification by ID
        /// </summary>
        [HttpGet("{id}", Name = "GetNotificationById")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation($"Getting notification with ID: {id}");

            if (id <= 0)
            {
                return NotFound(new { message = $"Notification with ID {id} not found" });
            }

            var notification = new Notifications
            {
                NotificationId = id,
                DriverId = 1,
                Type = "Safety",
                Title = "Test Notification",
                Message = "This is a test notification",
                Priority = "Medium",
                IsRead = false,
                SentAt = DateTime.Now
            };

            return Ok(notification);
        }

        /// <summary>
        /// GET: api/notifications/driver/{driverId}
        /// Get notifications for a specific driver
        /// </summary>
        [HttpGet("driver/{driverId}", Name = "GetNotificationsByDriver")]
        public IActionResult GetByDriverId(int driverId)
        {
            _logger.LogInformation($"Getting notifications for driver: {driverId}");

            var notifications = new List<Notifications>
            {
                new Notifications
                {
                    NotificationId = 1,
                    DriverId = driverId,
                    Type = "Safety",
                    Title = $"Notifications for Driver {driverId}",
                    Message = "Area-based alerts and notifications",
                    Priority = "High",
                    IsRead = false,
                    SentAt = DateTime.Now
                }
            };

            return Ok(notifications);
        }

        /// <summary>
        /// GET: api/notifications/driver/{driverId}/unread
        /// Get unread count for driver
        /// </summary>
        [HttpGet("driver/{driverId}/unread", Name = "GetUnreadCount")]
        public IActionResult GetUnreadCount(int driverId)
        {
            _logger.LogInformation($"Getting unread count for driver: {driverId}");

            // Mock count - replace with database query
            var unreadCount = 5;

            return Ok(new { driverId, unreadCount });
        }

        /// <summary>
        /// POST: api/notifications
        /// Create a new notification
        /// </summary>
        [HttpPost(Name = "CreateNotification")]
        public IActionResult Create([FromBody] CreateNotificationDto dto)
        {
            _logger.LogInformation($"Creating notification for driver {dto.DriverId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdNotification = new Notifications
            {
                NotificationId = 999, // Would be generated by database
                DriverId = dto.DriverId,
                Type = dto.Type,
                Title = dto.Title,
                Message = dto.Message,
                Priority = dto.Priority,
                IsRead = false,
                SentAt = DateTime.Now
            };

            return CreatedAtRoute(
                "GetNotificationById",
                new { id = createdNotification.NotificationId },
                createdNotification
            );
        }

        /// <summary>
        /// PUT: api/notifications/{id}/read
        /// Mark notification as read
        /// </summary>
        [HttpPut("{id}/read", Name = "MarkAsRead")]
        public IActionResult MarkAsRead(int id)
        {
            _logger.LogInformation($"Marking notification {id} as read");

            // TODO: Update in database
            return NoContent(); // 204 No Content
        }

        /// <summary>
        /// PUT: api/notifications/driver/{driverId}/mark-all-read
        /// Mark all notifications as read for a driver
        /// </summary>
        [HttpPut("driver/{driverId}/mark-all-read", Name = "MarkAllAsRead")]
        public IActionResult MarkAllAsRead(int driverId)
        {
            _logger.LogInformation($"Marking all notifications as read for driver {driverId}");

            // TODO: Update in database
            return NoContent();
        }

        /// <summary>
        /// DELETE: api/notifications/{id}
        /// Delete a notification
        /// </summary>
        [HttpDelete("{id}", Name = "DeleteNotification")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"Deleting notification {id}");

            // TODO: Delete from database
            return NoContent();
        }
    }

    // DTOs
    public class CreateNotificationDto
    {
        public int DriverId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
    }
}
