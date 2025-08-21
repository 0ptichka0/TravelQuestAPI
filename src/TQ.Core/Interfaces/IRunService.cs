using Ardalis.Result;
using TQ.Core.Aggregates.CPVisitsAggregate;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Interfaces
{
    public interface IRunService
    {
        Task<Result<IEnumerable<Run>>> GetListAsync();
        Task<Run> GetAsync(RunId id);
        Task<Result<Run>> CreateAsync(Run family);
        Task<Result<Run>> UpdateAsync(Run family);
        Task<Result> DeleteAsync(RunId id);
    }
}
