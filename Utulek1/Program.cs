using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utulek1.Application.Abstraction;
using Utulek1.Application.Implementation;
using Utulek1.Application.Services;
using Utulek1.Domain.Entities;
using Utulek1.Infrastructure;
using Utulek1.Infrastructure.Repositories;
using User = Utulek1.Domain.Entities.User;
using Serilog;

Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine($"[Serilog Error] {msg}"));

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("UtulekDb");
var serverVersion = ServerVersion.AutoDetect(connectionString);

builder.Services.AddDbContext<UtulekDbContext>(
    options => options.UseMySql(connectionString, serverVersion)
);


builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;

    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<UtulekDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; 
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddScoped<ICarouselRepository, CarouselRepository>();

builder.Services.AddScoped<ISystemLogRepository, SystemLogRepository>();
builder.Services.AddScoped<ISystemLogAppService, SystemLogAppService>();

builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<IAnimalAppService, AnimalAppService>();

builder.Services.AddScoped<IAdoptionAppService, AdoptionAppService>();

builder.Services.AddScoped<IUserAppService, UserAppService>();

builder.Services.AddScoped<IFileUploadService>(provider =>
{
    var env = provider.GetRequiredService<IWebHostEnvironment>();
    return new FileUploadService(env.WebRootPath);
});

builder.Services.AddSession();
builder.Services.AddScoped<ICarouselAppService, CarouselAppService>();
builder.Services.AddScoped<IHomeService, HomeService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbInitializer = services.GetRequiredService<IDbInitializer>();
    dbInitializer.Initialize();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
