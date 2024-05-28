using IMS.web.Models;
using Microsoft.AspNetCore.Identity;

namespace IMS.web.Data
{
    public class SeedingData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] Roles = { "SUPERADMIN", "ADMIN", "COUNTER", "STORE","PUBLIC" };
            
            foreach (string roleName in Roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            if (await _userManager.FindByNameAsync("superadmin@mail.com") == null)
            {
                var role = _roleManager.FindByNameAsync("SUPERADMIN").Result;
               
                var user = new ApplicationUser()
                {

                    FirstName = "Super",                    
                    LastName = "Admin",                    
                    StoreId = 0,                    
                    IsActive = true,
                    UserRoleId =role.Id,
                    UserName = "superadmin@mail.com",
                    Email = "superadmin@mail.com",
                    PhoneNumber = "1122334455",
                    Address = "Kathmandu",                    
                    CreatedBy = "superadmin",
                    CreatedDate = DateTime.Now
                };

                var res = await _userManager.CreateAsync(user, "Super@dmin1");
                await _userManager.SetLockoutEnabledAsync(user, false);
                if (res.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "SUPERADMIN");
                }
            }
        }
    }
}
