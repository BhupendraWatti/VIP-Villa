using System.Linq.Expressions;
using Villa_Services.Models;

namespace Villa_Services.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true);
        Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null);
        Task CreateAsync(Villa T); 
        Task  UpdateAsync(Villa T);
        Task DeleteAsync(Villa T);
        Task  SaveAsync();
    }
}
