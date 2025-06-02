using Villa_Services.Models;

namespace Villa_Services.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateAsync(VillaNumber T);
    }
}
