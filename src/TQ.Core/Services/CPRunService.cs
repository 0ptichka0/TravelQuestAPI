using Ardalis.Result;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.Core.Interfaces;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Services
{
    public class CPRunService : ICPRunService
    {
        private readonly ITravelQuestRepository<CPRun> _repository;
        public CPRunService(ITravelQuestRepository<CPRun> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<CPRun>>> GetListAsync()
        {
            var data = await _repository.ListAsync();

            return new Result<IEnumerable<CPRun>>(data);
        }

        public async Task<CPRun> GetAsync(IntId id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

        public async Task<Result<CPRun>> CreateAsync(CPRun data)
        {
            return await _repository.AddAsync(data);
        }
        
        public async Task<Result<CPRun>> UpdateAsync(CPRun data)
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
