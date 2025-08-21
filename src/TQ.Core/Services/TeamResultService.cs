using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.Core.Aggregates.TeamResultsAggregate;
using TQ.Core.Interfaces;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Services
{
    public class TeamResultService : ITeamResultService
    {
        private readonly ITravelQuestRepository<TeamResult> _repository;
        public TeamResultService(ITravelQuestRepository<TeamResult> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<TeamResult>>> GetListAsync()
        {
            var data = await _repository.ListAsync();

            return new Result<IEnumerable<TeamResult>>(data);
        }

        public async Task<TeamResult> GetAsync(IntId id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

        public async Task<Result<TeamResult>> CreateAsync(TeamResult data)
        {
            return await _repository.AddAsync(data);
        }

        public async Task<Result<TeamResult>> UpdateAsync(TeamResult data)
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
