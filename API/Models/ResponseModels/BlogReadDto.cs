﻿namespace API.Models.ResponseModels
{
    public class BlogReadDto
    {
        public Guid Id { get; set; }
        public string CreatorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
