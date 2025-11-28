using Microsoft.EntityFrameworkCore;
using Utulek1.Application.Abstraction;
using Utulek1.Application.Implementation;
using Utulek1.Application.Services;
using Utulek1.Infrastructure;
using Utulek1.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Pøipojení k databázi
var connectionString = builder.Configuration.GetConnectionString("UtulekDb");
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<UtulekDbContext>(
    options => options.UseMySql(connectionString, serverVersion)
);

// Registrace aplikacních služeb
builder.Services.AddScoped<IAnimalAppService, AnimalAppService>();

// Registrace FileUploadService s pøedáním webroot cesty
builder.Services.AddScoped<IFileUploadService>(provider =>
{
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    return new FileUploadService(env.WebRootPath);
});

// Session (pokud ji používáš)
builder.Services.AddSession();
builder.Services.AddScoped<ICarouselAppService, CarouselAppService>();
builder.Services.AddScoped<IHomeService, HomeService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Aktivace session
app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// MVC routování
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
