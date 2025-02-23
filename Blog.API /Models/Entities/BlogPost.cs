using System;

namespace Blog.API.Models.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }               // PK
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}