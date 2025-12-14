using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utulek1.Application.Abstraction;
using Utulek1.Application.Implementation;
using Utulek1.Application.Services;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure;
using Utulek1.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Pøipojení k databázi
var connectionString = builder.Configuration.GetConnectionString("UtulekDb");
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<UtulekDbContext>(
    options => options.UseMySql(connectionString, serverVersion)
);


// --- NOVÉ: KONFIGURACE IDENTITY ---
builder.Services.AddIdentity<User, Role>(options =>
{
    // Nastavení hesel (zjednodušené pro školní projekt)
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<UtulekDbContext>()
.AddDefaultTokenProviders();

// --- NOVÉ: KONFIGURACE COOKIES (Protože nemáme UI balíèek) ---
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Odkaz na Controller, který budeme tvoøit
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// --- NOVÉ: REGISTRACE SEEDERU ---
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

// Registrace aplikacních služeb
builder.Services.AddScoped<IAnimalAppService, AnimalAppService>();

builder.Services.AddScoped<IAdoptionAppService, AdoptionAppService>();

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

// --- NOVÉ: SPUŠTÌNÍ SEEDERU (Vytvoøení rolí a admina pøi startu) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbInitializer = services.GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}

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
