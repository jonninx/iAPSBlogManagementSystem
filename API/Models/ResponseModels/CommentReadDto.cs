namespace API.Models.ResponseModels
{
    public class CommentReadDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BlogPostId { get; set; }
    }
}
