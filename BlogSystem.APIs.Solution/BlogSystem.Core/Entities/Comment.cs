using BlogSystem.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities
{
    public class Comment : BaseClassWithId
    {
        
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        //[ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }


        //[ForeignKey("Author")]
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }

    }
}
