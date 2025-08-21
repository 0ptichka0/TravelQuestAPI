namespace TQ.SharedKernel.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEvents<TId>(IEnumerable<BaseEntity<TId>> entitiesWithEvents);
    }
}
