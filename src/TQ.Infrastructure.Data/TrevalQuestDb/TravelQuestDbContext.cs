using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.Core.Aggregates.CPVisitsAggregate;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Aggregates.TeamResultsAggregate;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.Core.Aggregates.UsersAggregate;
using TQ.Infrastructure.Data.TravelQuestDb.Configurations;
using TQ.SharedKernel;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Infrastructure.Data.TravelQuestDb
{
    /// <summary>
    /// cd C:\Users\pt1chka\source\repos\TravelQuestAPI
    /// dotnet ef migrations add Equipment --project src/TQ.Infrastructure.Data --startup-project TrevalQuest/TQ.Web.csproj --context TravelQuestDbContext
    /// 
    /// Add-Migration Equipment2 -StartupProject TQ.Web -Context TravelQuestDbContext -Project TQ.Infrastructure.Data
    /// 
    /// update-database -StartupProject TQ.Web -Context TravelQuestDbContext -Project TQ.Infrastructure.Data
    /// 
    /// 
    /// Создание: 
    /// dotnet ef migrations add newCPId --project src/TQ.Infrastructure.Data --startup-project TrevalQuest/TQ.Web.csproj --context TravelQuestDbContext
    /// 
    /// Применение: 
    /// update-database -StartupProject TQ.Web -Context TravelQuestDbContext -Project TQ.Infrastructure.Data
    /// </summary>
    public class TravelQuestDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;
        public TravelQuestDbContext(DbContextOptions<TravelQuestDbContext> options, IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }
        
        public DbSet<CP> CPs { get; set; }
        public DbSet<CPRun> CPsRuns { get; set; }
        public DbSet<CPVisit> CPVisits { get; set; }
        public DbSet<Run> Runs { get; set; }
        public DbSet<TeamResult> TeamResults { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<RunId>();
            modelBuilder.Ignore<CPId>();

            modelBuilder.ApplyConfiguration(new CPEntityConfiguration());   
            modelBuilder.ApplyConfiguration(new CPRunEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CPVisitEntityConfiguration());
            modelBuilder.ApplyConfiguration(new RunEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeamResultEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TeamEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (_dispatcher == null) return result;

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity<IntId>>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
