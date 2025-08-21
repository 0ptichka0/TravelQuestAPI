using Ardalis.Specification.EntityFrameworkCore;
using TQ.SharedKernel.Interfaces;

namespace TQ.Infrastructure.Data.TravelQuestDb
{
    public class TravelQuestRepository<T> : RepositoryBase<T>, IReadRepository<T>, ITravelQuestRepository<T> where T : class, IAggregateRoot
    {
        public TravelQuestRepository(TravelQuestDbContext dbContext) : base(dbContext)
        {

        }
    }
}
