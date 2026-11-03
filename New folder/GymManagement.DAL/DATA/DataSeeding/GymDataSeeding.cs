using GymManagement.DAL.DATA.DbContexts;
using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GymManagement.DAL.DATA.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDbContext dbContext , string seedFolderPath,ILogger logger ,CancellationToken ct =default)
        {
			try
			{
				if(!await dbContext.Plans.AnyAsync(ct))
				{
                    var plans = LoadDataFromJsonFile<Plan>(seedFolderPath, "plans.json");
                    if(plans.Any())
                    {
                        dbContext.Plans.AddRange(plans);

                    }
                    await dbContext.SaveChangesAsync();
                    logger.LogInformation("Database Seeded");

				}
			}
			catch (Exception ex)
			{
                logger.LogError($"Database Seeding Failed : {ex.Message}");
                throw;
			}

        }

		private static List<T> LoadDataFromJsonFile<T>(string folderPath , string fileName)
		{
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed Data File Not Found : {filePath}");


            var data = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            var entities = JsonSerializer.Deserialize<List<T>>(data, options) ?? [];

            return entities;
        }
    }
}
