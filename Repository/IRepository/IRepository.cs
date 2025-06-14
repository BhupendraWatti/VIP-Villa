using System.Linq.Expressions;
using Villa_Services.Models;

namespace Villa_Services.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task CreateAsync(T T);
        
        Task DeleteAsync(T T);
        Task SaveAsync();
    }
}
