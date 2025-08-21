using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TQ.Infrastructure.Data.TravelQuestDb
{
    public class TravelQuestDbContextFactory : IDesignTimeDbContextFactory<TravelQuestDbContext>
    {
        public TravelQuestDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TravelQuestDbContext>();

            var connectionString = config.GetConnectionString("TravelQuestDbConnection");
            optionsBuilder.UseNpgsql(connectionString);

            return new TravelQuestDbContext(optionsBuilder.Options, dispatcher: null); // dispatcher временно не нужен
        }
    }
}
