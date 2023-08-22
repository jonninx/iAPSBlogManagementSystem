namespace API.Models.RequestModels
{
    public class BlogCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatorId { get; set; }
    }
}
