using BlogSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications
{
    public class PostSpecificationWithAllIncludes : BaseSpecifications<Post>
    {


        public PostSpecificationWithAllIncludes(PostSpecificationParams Parms = null) : base()
        {
            Includes.Add(P => P.Author);
            Includes.Add(P => P.Tags);
            Includes.Add(P => P.Category);
            SearchFiltration(Parms);
            SortFiltration(Parms);
            ApplyPagination(Parms);

        }

        private void ApplyPagination(PostSpecificationParams Parms)
        {
            skip = (Parms.index - 1) * Parms.pageSize;
            take = Parms.pageSize;
            ApplyingPagination = true;
        }

        private void SearchFiltration(PostSpecificationParams Parms)
        {
            Criteria = P =>
            (string.IsNullOrEmpty(Parms.Search) ||
               (P.Title.ToLower().Contains(Parms.Search.ToLower()))
            || (P.Category.Name.ToLower().Contains(Parms.Search.ToLower()))
            || (P.Content.ToLower().Contains(Parms.Search.ToLower()))
            //||(P.Author.UserName.ToLower().Contains(Parms.Search.ToLower()))   // not required
            || (P.Tags.Any(T => T.Name.ToLower().Contains(Parms.Search.ToLower()))))
            && (Parms.status == null || Parms.status == P.Status)
            &&(string.IsNullOrEmpty(Parms.Tag) || P.Tags.Any(T=>T.Name==Parms.Tag))
            &&(string.IsNullOrEmpty(Parms.Category) || P.Category.Name == Parms.Category)
            ;
        }
        private void SortFiltration(PostSpecificationParams Parms)
        {
            switch (Parms?.Sort?.ToLower())
            {
                case "title":
                    OrderBy = p => p.Title; break;
                case "titledesc":
                    OrderByDesc = p => p.Title; break;
                case "author":
                    OrderBy = p => p.Author; break;
                case "authordesc":
                    OrderByDesc = p => p.Author; break;
            }
        }


        public PostSpecificationWithAllIncludes(int id) : base(T => T.Id == id)
        {
            Includes.Add(P => P.Author);
            Includes.Add(P => P.Tags);
            Includes.Add(P => P.Category);
        }

        

    }
}
