using BlogSystem.Core.Entities;
using BlogSystem.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseClassWithId
    {
         Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllSpecAsync(ISpecifications<T> Spec);
        Task<T> GetByIdSpecAsync(ISpecifications<T>Spec);
        Task AddAsync(T item);
        Task DeleteAsync(T item);
        Task UpdateAsync(T entity);

    }
}
