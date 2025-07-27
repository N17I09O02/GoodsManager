using Microsoft.EntityFrameworkCore;
using Web_Manage.Models;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    public DbSet<DonHang> DonHangs { get; set; }
    public DbSet<LanDatHang> LanDatHangs { get; set; }
    public DbSet<ChiTietDatHang> ChiTietDatHangs { get; set; }
    public DbSet<LichSuMatHang> LichSuMatHangs { get; set; }
    public DbSet<MatHang> MatHangs { get; set; }
    public DbSet<KhachHang> KhachHangs { get; set; }
    public DbSet<DoanhNghiep> DoanhNghieps { get; set; }
    public DbSet<Image> images { get; set; }
    public DbSet<NguyenLieu> nguyenLieus { get; set; }
    public DbSet<DoiTac> doiTacs { get; set; }
    public DbSet<DoiTacNguyenLieu> doiTacNguyenLieus { get; set; }
}

