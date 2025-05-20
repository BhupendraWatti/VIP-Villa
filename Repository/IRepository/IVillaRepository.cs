using System.Linq.Expressions;
using Villa_Services.Models;

namespace Villa_Services.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa T);
    }
}
