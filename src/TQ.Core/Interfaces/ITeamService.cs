using Ardalis.Result;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Interfaces
{
    public interface ITeamService
    {
        Task<Result<IEnumerable<Team>>> GetListAsync();
        Task<Team> GetAsync(IntId id);
        Task<Result<Team>> CreateAsync(Team family);
        Task<Result<Team>> UpdateAsync(Team family);
        Task<Result> DeleteAsync(IntId id);
    }
}
