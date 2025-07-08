using Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Seeder
{
	public class AppSeeder
	{
		public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<DatabaseContext>();

					await context.Database.MigrateAsync();

					await CourseSeeder.SeedCoursesAsync(context, 30);

					Console.WriteLine("Database seeding process completed successfully.");
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<AppSeeder>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
					//throw;
				}
			}
		}
	}
}
