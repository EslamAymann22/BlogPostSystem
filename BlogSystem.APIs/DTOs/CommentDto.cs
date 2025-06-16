using System.ComponentModel.DataAnnotations;

namespace BlogSystem.APIs.DTOs
{
    public class CommentDto
    {

        [Required]
        public string Content { get; set; }
        [Required]
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? AuthorName { get; set; }

    }
}
