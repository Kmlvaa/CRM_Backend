using CRM.Entities;
using CRM.Enums;
using Microsoft.AspNetCore.Identity;

namespace CRM.Helper
{
    public class DataSeed
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {

            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string role = RoleType.Admin.ToString();

                var exists = await roleManager.RoleExistsAsync(role);
                if (!exists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                var user = new User
                {
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    Role = RoleType.Admin,
                };

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var existingUser = await userManager.FindByNameAsync("Admin");
                if (existingUser is not null) return;

                await userManager.CreateAsync(user, "Admin123");
                await userManager.AddToRoleAsync(user, role);

                return;
            }
        }
    }
}
