using Villa_Services.Data;
using Villa_Services.Models;
using Villa_Services.Repository.IRepository;

namespace Villa_Services.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<VillaNumber> UpdateAsync(VillaNumber T)
        {
            T.UpdateDate = DateTime.Now;
            _db.VillaNo.Update(T);
            await _db.SaveChangesAsync();
            return T;
        }
    }
}
