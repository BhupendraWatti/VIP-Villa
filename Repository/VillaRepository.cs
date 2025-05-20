using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa_Services.Data;
using Villa_Services.Models;
using Villa_Services.Repository.IRepository;

namespace Villa_Services.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        
        public VillaRepository(ApplicationDbContext db):base (db) 
        {
            _db = db;
            
        }

        public async Task<Villa> UpdateAsync(Villa T)
        {
            T.UpdatedDate = DateTime.Now;
            _db.Villas.Update(T);
            await _db.SaveChangesAsync();
            return T;
        }
    }
}
