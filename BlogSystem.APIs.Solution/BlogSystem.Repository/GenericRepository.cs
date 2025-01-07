using BlogSystem.Core.Entities;
using BlogSystem.Core.Repositories;
using BlogSystem.Core.Specifications;
using BlogSystem.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository
{

    public class GenericRepository<T> : IGenericRepository<T> where T : BaseClassWithId
    {
        private readonly DbContextIdentity _dbContext;

        public GenericRepository(DbContextIdentity dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IEnumerable<T>> GetAllAsync()
              => await _dbContext.Set<T>().Select(T => T).ToListAsync();
        public async Task<T> GetByIdAsync(int id)
        =>await _dbContext.Set<T>().Where(T=>T.Id==id).FirstOrDefaultAsync();


        public async Task<IEnumerable<T>> GetAllSpecAsync(ISpecifications<T> Spec)
        {
            return await PrepareSpecification(Spec).ToListAsync();
        }

        public async Task<T> GetByIdSpecAsync(ISpecifications<T> Spec)
        {
            return await PrepareSpecification(Spec).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T item) { 
           await _dbContext.Set<T>().AddAsync(item);
           await _dbContext.SaveChangesAsync();
        }

        private IQueryable<T> PrepareSpecification(ISpecifications<T> Spe)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spe);
        }



        ///public async Task<IEnumerable<T>> GetAllAsync()
        ///{
        ///    if (typeof(T) == typeof(Post))
        ///    {
        ///        return  await _dbContext.Set<Post>()
        ///            .Include(U => U.Category)
        ///            .Include(U => U.Tags)
        ///            .Include(U => U.Author)
        ///            .Select(U => U)
        ///            .ToListAsync() as List<T>;
        ///    }
        ///    return await _dbContext.Set<T>().Select(T => T).ToListAsync();
        ///}


    }
}
