using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseClassWithId
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
                    = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int take { get; set; }
        public int skip { get; set; }
        public bool ApplyingPagination {  get; set; } = true;
        public int countOfElements {  get; set; }

        public BaseSpecifications() { }
        public BaseSpecifications(Expression<Func<T, bool>> _Criteria)
        {
            Criteria = _Criteria;
        }

    }
}
