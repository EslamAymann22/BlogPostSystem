using BlogSystem.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.APIs.DTOs
{
    public class PostDtoToReturn
    {
        public int Id{ get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }  
        [Required]
        public string Status { get; set; } 
        [Required]
        public List<string> Tags { get; set; } 
        [Required]
        public int CategoryId { get; set; }
        public string Category { get; set; }

    }
}
