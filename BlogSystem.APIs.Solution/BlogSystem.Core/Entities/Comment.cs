using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        //[ForeignKey("Post")]
        public int PostId { get; set; }
        public BlogPost Post { get; set; }


        //[ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User Author { get; set; }

    }
}
