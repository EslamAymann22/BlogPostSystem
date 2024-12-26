using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications
{
    public class PostSpecificationWithAllIncludes : BaseSpecifications<Post>
    {

        public PostSpecificationWithAllIncludes() : base()
        {
            Includes.Add(P => P.Author);
            Includes.Add(P => P.Tags);
            Includes.Add(P => P.Category);
        }
        public PostSpecificationWithAllIncludes(int id) : base(T => T.Id == id)
        {
            Includes.Add(P => P.Author);
            Includes.Add(P => P.Tags);
            Includes.Add(P => P.Category);
        }
    }
}
