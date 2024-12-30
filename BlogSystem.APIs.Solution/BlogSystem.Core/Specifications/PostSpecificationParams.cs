using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications
{
    public class PostSpecificationParams 
    {
        public string? Sort {  get; set; }
        public string? Search {  get; set; }
        public PostStatus? status {  get; set; }
        //public string Sort {  get; set; }



    }
}
