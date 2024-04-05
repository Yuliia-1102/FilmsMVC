using DocumentFormat.OpenXml.InkML;
using FilmsInfrastructure.Models;
using Microsoft.AspNetCore.Identity;
using FilmsDomain.Model;

namespace FilmsInfrastructure
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DbfilmsContext context)
        {
            string adminEmail = "admin@gmail.com";
            string password = "yuliia123";
            string name = "Admin";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail, Name = name };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                    Customer c = new Customer { Email = adminEmail, Name = name };
                    context.Add(c);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
