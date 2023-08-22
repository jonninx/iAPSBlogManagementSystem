namespace API.Models.ResponseModels
{
    public class NotificationDto
    {
        public Guid NotificationId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
