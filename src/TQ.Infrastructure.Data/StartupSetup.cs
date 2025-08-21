using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TQ.Infrastructure.Data.TravelQuestDb;

namespace TQ.Infrastructure.Data
{
    public static class StartupSetup
    {
        public static void AddTravelQuestDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<TravelQuestDbContext>(options =>
                options.UseNpgsql(connectionString).ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning)));

    }
}
