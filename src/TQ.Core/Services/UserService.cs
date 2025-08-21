using Ardalis.Result;
using TQ.Core.Aggregates.UsersAggregate;
using TQ.Core.Interfaces;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ITravelQuestRepository<User> _repository;
        public UserService(ITravelQuestRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<Result<IEnumerable<User>>> GetListAsync()
        {
            var user = await _repository.ListAsync();

            return new Result<IEnumerable<User>>(user);
        }

        public async Task<User> GetAsync(IntId id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user;
        }

        public async Task<Result<User>> CreateAsync(User user)
        {
            return await _repository.AddAsync(user);
        }

        public async Task<Result<User>> UpdateAsync(User user)
        {
            await _repository.UpdateAsync(user);
            return user;
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
