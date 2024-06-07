using IMS.web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using IMS.Infrastructure.Services;
using IMS.Infrastructure;
using IMS.Infrastructure.IRepository;
using IMS.Infrastructure.Repository.CRUD;
using IMS.Infrastructure.Repository;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

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



builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddTransient(typeof(ICrudService<>), typeof(CrudService<>));
builder.Services.AddTransient<IRawSqlRepository, RawSqlRepository>();
builder.Services.AddTransient<IGenericRepository, GenericRepository>();

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


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
