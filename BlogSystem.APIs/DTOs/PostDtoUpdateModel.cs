using BlogSystem.Core.Entities.Identity;
using BlogSystem.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.APIs.DTOs
{
    public class PostDtoUpdateModel
    {
        [Required]
        public int Id { get; set; } 
        public string? Title { get; set; }
        public string? Content { get; set; }

        public string? AuthorId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public PostStatus? Status { get; set; }

        public List<string>? Tags { get; set; } = new List<string>();

        public int? CategoryId { get; set; }

    }
}
