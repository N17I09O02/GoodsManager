using Microsoft.EntityFrameworkCore;
using Web_Manage.Models;

namespace Web_Manage.Services
{
    public class DoiTacService
    {
        private readonly ApplicationDbContext _context;
        public DoiTacService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DoiTac?> GetByIdAsync(int id)
        {
            return await _context.doiTacs.FindAsync(id);
        }

        public async Task<List<DoiTac>> GetAllAsync()
        {
            return await _context.doiTacs.ToListAsync();
        }

        public async Task AddAsync(DoiTac doiTac)
        {
            _context.doiTacs.Add(doiTac);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(DoiTac doiTac)
        {
            _context.doiTacs.Update(doiTac);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doiTac = await _context.doiTacs.FindAsync(id);
            if (doiTac == null)
                throw new Exception($"Không tìm thấy đối tác với Id = {id}");

            _context.doiTacs.Remove(doiTac);
            await _context.SaveChangesAsync();
        }
    }
}
