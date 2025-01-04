using BlogSystem.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogSystem.APIs.DTOs
{
    public class PostDtoToReturn
    {
        public int Id{ get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }  
        public string Status { get; set; } 
        public List<string> Tags { get; set; } 
        public int CategoryId { get; set; }
        public string Category { get; set; }

    }
}
