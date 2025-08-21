using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.SharedKernel;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Aggregates.CPVisitsAggregate
{
    public class CPVisit : BaseEntity<IntId>, IAggregateRoot
    {
        /// <summary>
        /// id команды
        /// </summary>
        public IntId TeamId { get; private set; }
        /// <summary>
        /// id КП
        /// </summary>
        public CPId CPId { get; private set; }
        /// <summary>
        /// Подходит ли точка
        /// </summary>
        public bool IsValid { get; private set; }
        /// <summary>
        /// Время визита (от начала забега)
        /// </summary>
        public TimeSpan VisitTime { get; private set; }

        public CPVisit() { }
        public CPVisit(IntId teamId, CPId cPId, bool isValid, TimeSpan visitTime)
        {
            TeamId = teamId;
            CPId = cPId;
            IsValid = isValid;
            VisitTime = visitTime;
        }
        public void UpdateTeamId(IntId teamId)
        {
            TeamId = teamId;
        }
        public void UpdateCPId(CPId cPId)
        {
            CPId = cPId;
        }
        public void UpdateIsValid(bool isValid)
        {
            IsValid = isValid;
        }
        public void UpdateVisitTime(TimeSpan visitTime)
        {
            VisitTime = visitTime;
        }
    }
}
