using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications.Params
{
    public class CommentSpecificationParams
    {
        //public int PostId{  get; set; }
        private int _PageSize { get; set; } = 5;
        public int Index { get; set; } = 1;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = Math.Min(5, Math.Max(1, value));}
        }

    }
}
