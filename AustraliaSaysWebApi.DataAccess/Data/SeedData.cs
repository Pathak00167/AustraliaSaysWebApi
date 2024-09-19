using AustraliaSaysWebApi.DataAccess.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AustraliaSaysWebApi.DataAccess.Data
{
    public class SeedData
    {
        public static async Task RoleIntializer(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var roleName in new[] { "Admin", "User" })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
            if (adminUser == null)
            {

                await InsertAdminData(userManager);
            }
            else
            {

                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        private static async Task InsertAdminData(UserManager<ApplicationUser> userManager)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
            };
            var result = await userManager.CreateAsync(adminUser, "Pathak123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {

                Console.WriteLine("Failed to create admin user");
            }
        }
    }
}
