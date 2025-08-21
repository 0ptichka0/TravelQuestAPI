using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.SharedKernel;
using TQ.SharedKernel.Interfaces;

namespace TQ.Core.Aggregates.RunsAggregate
{
    public class Run : BaseEntity<RunId>, IAggregateRoot
    {
        /// <summary>
        /// Название команды
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Дата и время старта
        /// </summary>
        public DateTime RunStart { get; private set; }
        /// <summary>
        /// Продолжительность соревнований (hh:mm:ss)
        /// </summary>
        public TimeSpan Duration { get; private set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; private set; }

        public Run() { }
        public Run(RunId id, string name, DateTime runStart, TimeSpan duration, string? description)
        {
            Id = id;
            Name = name;
            RunStart = runStart;
            Duration = duration;
            Description = description;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
        public void UpdateRunStart(DateTime runStart)
        {
            RunStart = runStart;
        }
        public void UpdateDuration(TimeSpan duration) 
        {
            Duration = duration;
        }
        public void UpdateDescription(string? description)
        {
            Description = description;
        }
    }
}
