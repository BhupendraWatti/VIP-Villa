using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villa_Services.Data;
using Villa_Services.Models;
using Villa_Services.Repository.IRepository;

namespace Villa_Services.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Villa T)
        {
           await _db.Villas.AddAsync(T);
           await SaveAsync();

        }

        public async Task DeleteAsync(Villa T)
        {
            _db.Villas.Remove(T);
            await SaveAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _db.Villas;
            if (!tracked)
            {
                query = query.AsNoTracking();    
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();

        }


        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
        {
            IQueryable<Villa> query= _db.Villas;
            if (filter != null) {
                query = query.Where(filter);
            }
            return await query.ToListAsync();

        }

        public async Task SaveAsync()
        {
           await _db.SaveChangesAsync();    
        }

        public  async Task UpdateAsync(Villa T)
        {
             _db.Villas.Update(T);
            await SaveAsync();
        }

        
    }
}
