using Ardalis.Specification;

namespace TQ.SharedKernel.Interfaces
{
    public interface ITravelQuestRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}
