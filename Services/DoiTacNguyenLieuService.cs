using Microsoft.EntityFrameworkCore;
using Web_Manage.Models;

namespace Web_Manage.Services
{
    public class DoiTacNguyenLieuService
    {
        private readonly ApplicationDbContext _context;
        public DoiTacNguyenLieuService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<DoiTacNguyenLieu?> GetAsync(int id)
        {
            return await _context.doiTacNguyenLieus.FindAsync(id);
        }

        public async Task<List<DoiTacNguyenLieu>> GetByIdDoiTacAsync(int idDoiTac)
        {
            return await _context.doiTacNguyenLieus
                .Where(dtnl => dtnl.IdDoiTac == idDoiTac)
                .ToListAsync();
        }


        public async Task<List<DoiTacNguyenLieu>> GetByIdNguyenLieuAsync(int idNguyenLieu)
        {
            return await _context.doiTacNguyenLieus
               .Where(dtnl => dtnl.IdNguyenLieu == idNguyenLieu)
                .ToListAsync();
        }

        public async Task<DoiTacNguyenLieu?> GetByIdAsync(int idDoiTac, int idNguyenLieu)
        {
            return await _context.doiTacNguyenLieus
                .FirstOrDefaultAsync(x => x.IdDoiTac == idDoiTac && x.IdNguyenLieu == idNguyenLieu);
        }


        public async Task<List<DoiTacNguyenLieu>> GetAllAsync()
        {
            return await _context.doiTacNguyenLieus.ToListAsync();
        }
        public async Task AddAsync(DoiTacNguyenLieu doiTacNguyenLieu)
        {
            _context.doiTacNguyenLieus.Add(doiTacNguyenLieu);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DoiTacNguyenLieu doiTacNguyenLieu)
        {
            _context.doiTacNguyenLieus.Update(doiTacNguyenLieu);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var doiTacNguyenLieu = await _context.doiTacNguyenLieus.FindAsync(id);
            if (doiTacNguyenLieu == null)
                throw new Exception($"Không tìm thấy đối tác nguyên liệu với Id = {id}");

            _context.doiTacNguyenLieus.Remove(doiTacNguyenLieu);
            await _context.SaveChangesAsync();
        }
    }
}
