using Ardalis.Result;
using TQ.Core.Aggregates.UsersAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Interfaces
{
    public interface IUserService
    {
        Task<Result<IEnumerable<User>>> GetListAsync();
        Task<User> GetAsync(IntId id);
        Task<Result<User>> CreateAsync(User family);
        Task<Result<User>> UpdateAsync(User family);
        Task<Result> DeleteAsync(IntId id);
    }
}
