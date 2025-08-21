using Ardalis.Result;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.Core.Interfaces;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITravelQuestRepository<Team> _repository;
        public TeamService(ITravelQuestRepository<Team> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<Team>>> GetListAsync()
        {
            var data = await _repository.ListAsync();

            return new Result<IEnumerable<Team>>(data);
        }

        public async Task<Team> GetAsync(IntId id)
        {
            var data = await _repository.GetByIdAsync(id);
            return data;
        }

        public async Task<Result<Team>> CreateAsync(Team data)
        {
            return await _repository.AddAsync(data);
        }

        public async Task<Result<Team>> UpdateAsync(Team data)
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
