using IMS.Infrastructure;
using IMS.Infrastructure.IRepository;
using IMS.Infrastructure.Repository;
using IMS.Infrastructure.Repository.CRUD;
using IMS.Infrastructure.Services;
using IMS.web.Data;
using IMS.web.Middleware;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using StackExchange.Redis;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/ims-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDbContext<IMSDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
 e => e.MigrationsAssembly("IMS.web")));


//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//  .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var redisConnection = builder.Configuration.GetConnectionString("Redis");

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddTransient(typeof(ICrudService<>), typeof(CrudService<>));
builder.Services.AddTransient<IRawSqlRepository, RawSqlRepository>();
builder.Services.AddTransient<IGenericRepository, GenericRepository>();
builder.Services.AddTransient<IRedisService, RedisService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("SUPERADMIN", policy => policy.RequireRole("SUPERADMIN"));
//    options.AddPolicy("ADMIN", policy => policy.RequireRole("ADMIN"));
//    options.AddPolicy("COUNTER", policy => policy.RequireRole("COUNTER"));
//    options.AddPolicy("STORE", policy => policy.RequireRole("STORE"));
//    options.AddPolicy("PUBLIC", policy => policy.RequireRole("PUBLIC"));
//});


//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
//});
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    // Add other logging providers as needed.
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedingData.InitializeAsync(services);
}

app.UseMiddleware<GlobalExceptionMiddleware>();
// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//}
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
