using Ardalis.Result;
using TQ.Core.Aggregates.CPVisitsAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Interfaces
{
    public interface ICPVisitService
    {
        Task<Result<IEnumerable<CPVisit>>> GetListAsync();
        Task<CPVisit> GetAsync(IntId id);
        Task<Result<CPVisit>> CreateAsync(CPVisit family);
        Task<Result<CPVisit>> UpdateAsync(CPVisit family);
        Task<Result> DeleteAsync(IntId id);
    }
}
