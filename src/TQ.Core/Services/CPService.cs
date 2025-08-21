using Ardalis.Result;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.SharedKernel.Interfaces;

namespace TQ.Core.Services
{
    public class CPService : ICPService
    {
        private readonly ITravelQuestRepository<CP> _repository;
        public CPService(ITravelQuestRepository<CP> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<CP>>> GetListAsync()
        {
            var data = await _repository.ListAsync();

            return new Result<IEnumerable<CP>>(data);
        }

        public async Task<CP> GetAsync(CPId id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

        public async Task<Result<CP>> CreateAsync(CP data)
        {
            return await _repository.AddAsync(data);
        }

        public async Task<Result<CP>> UpdateAsync(CP data)
        {
            await _repository.UpdateAsync(data);
            return data;
        }

        public async Task<Result> DeleteAsync(CPId id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null) return Result.NotFound();
            await _repository.DeleteAsync(result);
            return Result.Success();
        }
    }
}
