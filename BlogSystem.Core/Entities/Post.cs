﻿using BlogSystem.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Entities
{
    public class Post : BaseClassWithId
    {

        public string Title { get; set; }
        public string Content { get; set; }

        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Published;

        public List<Tag>Tags { get; set; } = new List<Tag>();

        [ForeignKey("category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
