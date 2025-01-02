using BlogSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications
{
    public static class SpecificationsEvaluator<T> where T : BaseClassWithId
    {
        
        public static IQueryable<T> GetQuery(IQueryable<T> inputQ, ISpecifications<T> spec)
        {

            var Query = inputQ;

            if (spec.Criteria is not null)
                Query = Query.Where(spec.Criteria);

            if(spec.OrderBy is not null)
                Query = Query.OrderBy(spec.OrderBy);

            if (spec.OrderByDesc is not null)
                Query = Query.OrderByDescending(spec.OrderByDesc);

            spec.countOfElements = Query.Count();

            if(spec.ApplyingPagination)
                Query = Query.Skip(spec.skip).Take(spec.take);


            Query = spec.Includes.Aggregate(Query, (A, B) => A.Include(B));

            return Query;

        }


    }
}
