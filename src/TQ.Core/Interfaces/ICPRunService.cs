using Ardalis.Result;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Interfaces
{
    public interface ICPRunService
    {
        Task<Result<IEnumerable<CPRun>>> GetListAsync();
        Task<CPRun> GetAsync(IntId id);
        Task<Result<CPRun>> CreateAsync(CPRun family);
        Task<Result<CPRun>> UpdateAsync(CPRun family);
        Task<Result> DeleteAsync(IntId id);
    }
}
