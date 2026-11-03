using GymManagement.DAL.DATA.DataSeeding;
using GymManagement.DAL.DATA.DbContexts;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL
{
    public  static  class ProgramExtensions
    {

        public static async Task MigrateAndSeedDataBaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var roleManager=scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager=scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();




            var pendingMigrations = await dbcontext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Applying {pendingMigrations.Count()} Pending Migrations");
                await dbcontext.Database.MigrateAsync();
            }


            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbcontext, seedFolderPath, logger);
            await IdentityDataSeeding.SeedIdentityDataAsync(roleManager,userManager,logger);

        }
    }
}
