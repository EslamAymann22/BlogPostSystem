using BlogSystem.Core.Entities;
using BlogSystem.Core.Specifications.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BlogSystem.Core.Specifications
{
    public class CommentSpecificationWithAuthInclude : BaseSpecifications<Comment>
    {


        public CommentSpecificationWithAuthInclude(CommentSpecificationParams _params, int id) : base(C => C.PostId == id)
        {
           
            skip = (_params.Index - 1) * _params.PageSize;
            take = _params.PageSize;
            ApplyingPagination = true;
        }

        public CommentSpecificationWithAuthInclude(int id):base(C=>C.Id==id)
        {
            Includes.Add(C => C.Author);
        }

    }
}
