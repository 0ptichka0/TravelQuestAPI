using Ardalis.Result;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;

namespace TQ.Core.Interfaces
{
    public interface ICPService
    {
        Task<Result<IEnumerable<CP>>> GetListAsync();
        Task<CP> GetAsync(CPId id);
        Task<Result<CP>> CreateAsync(CP family);
        Task<Result<CP>> UpdateAsync(CP family);
        Task<Result> DeleteAsync(CPId id);
    }
}
