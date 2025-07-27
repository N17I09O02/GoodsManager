using Microsoft.EntityFrameworkCore;
using Web_Manage;
using Web_Manage.Services;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MatHangService>();
builder.Services.AddScoped<KhachHangService>();
builder.Services.AddScoped<DonHangService>();
builder.Services.AddScoped<LanDatHangService>();
builder.Services.AddScoped<ChiTietDatHangService>();
builder.Services.AddScoped<DoanhNghiepService>();
builder.Services.AddScoped<DoiTacService>();
builder.Services.AddScoped<DoiTacNguyenLieuService>();
builder.Services.AddScoped<NguyenLieuService>();

builder.Services.AddDefaultAWSOptions(new AWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<S3Service>();


builder.Services.AddHttpClient(); 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 41))
    )
);

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
