using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications.Params
{
    public class PostSpecificationParams
    {
        public string? Sort { get; set; }
        private string? _Search;
        public string? Search
        {
            get { return _Search; }
            set { _Search = value?.ToLower(); }
        }

        public PostStatus? status { get; set; }

        private int _PageSize = 5;
        public int pageSize
        {
            get { return _PageSize; }
            set { _PageSize = Math.Min(Math.Max(1, value), 5); }
        }
        public int index { get; set; } = 1;
        public string? Tag { get; set; }
        public string? Category { get; set; }
    }
}
