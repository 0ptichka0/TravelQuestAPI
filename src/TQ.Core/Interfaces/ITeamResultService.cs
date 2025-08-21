using Ardalis.Result;
using TQ.Core.Aggregates.TeamResultsAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Interfaces
{
    public interface ITeamResultService
    {
        Task<Result<IEnumerable<TeamResult>>> GetListAsync();
        Task<TeamResult> GetAsync(IntId id);
        Task<Result<TeamResult>> CreateAsync(TeamResult family);
        Task<Result<TeamResult>> UpdateAsync(TeamResult family);
        Task<Result> DeleteAsync(IntId id);
    }
}
