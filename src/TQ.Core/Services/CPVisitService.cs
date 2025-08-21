using Ardalis.Result;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.Core.Aggregates.CPVisitsAggregate;
using TQ.Core.Interfaces;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Services
{
    public class CPVisitService : ICPVisitService
    {
        private readonly ITravelQuestRepository<CPVisit> _repository;
        public CPVisitService(ITravelQuestRepository<CPVisit> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<CPVisit>>> GetListAsync()
        {
            var data = await _repository.ListAsync();

            return new Result<IEnumerable<CPVisit>>(data);
        }

        public async Task<CPVisit> GetAsync(IntId id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

        public async Task<Result<CPVisit>> CreateAsync(CPVisit data)
        {
            return await _repository.AddAsync(data);
        }

        public async Task<Result<CPVisit>> UpdateAsync(CPVisit data)
        {
            await _repository.UpdateAsync(data);
            return data;
        }

        public async Task<Result> DeleteAsync(IntId id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null) return Result.NotFound();
            await _repository.DeleteAsync(result);
            return Result.Success();
        }
    }
}
