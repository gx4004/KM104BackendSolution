using System;

namespace Blog.API.Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }                 // PK
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = false;

        // Hangi blog yazısına ait
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

        // Yorumu yazan kullanıcının ID’si
        public int UserId { get; set; }
    }
}