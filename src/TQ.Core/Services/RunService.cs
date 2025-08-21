using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Interfaces;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Services
{
    public class RunService : IRunService
    {
        private readonly ITravelQuestRepository<Run> _repository;
        public RunService(ITravelQuestRepository<Run> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<Run>>> GetListAsync()
        {
            var data = await _repository.ListAsync();

            return new Result<IEnumerable<Run>>(data);
        }

        public async Task<Run> GetAsync(RunId id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

        public async Task<Result<Run>> CreateAsync(Run data)
        {
            return await _repository.AddAsync(data);
        }

        public async Task<Result<Run>> UpdateAsync(Run data)
        {
            await _repository.UpdateAsync(data);
            return data;
        }

        public async Task<Result> DeleteAsync(RunId id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null) return Result.NotFound();
            await _repository.DeleteAsync(result);
            return Result.Success();
        }
    }
}
