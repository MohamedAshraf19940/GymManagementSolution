using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace GymManagement.DAL.DATA.DataSeeding
{
    public class IdentityDataSeeding
    {
        public static async Task SeedIdentityDataAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger logger,
            CancellationToken ct = default)
        {
            try
            {
                string[] roles = { "SuperAdmin", "Admin" };

                foreach (var roleName in roles)
                {
                    if (await roleManager.RoleExistsAsync(roleName))
                        continue;

                    var result = await roleManager.CreateAsync(
                        new IdentityRole(roleName));

                    if (!result.Succeeded)
                    {
                        logger.LogError(
                            "Failed to create role {Role}: {Errors}",
                            roleName,
                            string.Join(", ",
                                result.Errors.Select(e => e.Description)));
                    }
                }

                logger.LogInformation(
                    "Identity roles seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Identity Seeding Failed");
                throw;
            }
        }
    }
}