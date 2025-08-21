using System.ComponentModel.DataAnnotations.Schema;

namespace TQ.SharedKernel
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }

        /// <summary>
        /// Когда создан
        /// </summary>
        //public DateTime CreatedAt { get; protected set; }
        /// <summary>
        /// Кем создан
        /// </summary>
        //public string CreatedBy { get; protected set; }

        private List<BaseDomainEvent> _domainEvents = new();
        [NotMapped]
        public IEnumerable<BaseDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RegisterDomainEvent(BaseDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        internal void ClearDomainEvents() => _domainEvents.Clear();
    }
}
