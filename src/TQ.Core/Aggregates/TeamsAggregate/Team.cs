using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;
using TQ.SharedKernel;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;

namespace TQ.Core.Aggregates.TeamsAggregate
{
    public class Team : BaseEntity<IntId>, IAggregateRoot
    {
        /// <summary>
        /// id забега
        /// </summary>
        public RunId RunId { get; private set; }
        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime RegistrationDate { get; private set; }
        /// <summary>
        /// Название команды
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Код команды
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// Територия
        /// </summary>
        public string Area { get; private set; }
        /// <summary>
        /// группа
        /// </summary>
        public string Group { get; private set; }

        public Team() { }
        public Team(RunId runId, DateTime registrationDate, string name, string code, string area, string group) 
        {
            RunId = runId;
            RegistrationDate = registrationDate;
            Name = name;
            Code = code;
            Area = area;
            Group = group;
        }

        public void UpdateRunId(RunId runId)
        {
            RunId = runId;
        }
        public void UpdateRegistrationDate(DateTime registrationDate)
        {
            RegistrationDate = registrationDate;
        }
        public void UpdateName(string name)
        {
            Name = name;
        }
        public void UpdateCode(string code)
        {
            Code = code;
        }
        public void UpdateArea(string area)
        {
            Area = area;
        }
        public void UpdateGroup(string group)
        {
            Group = group;
        }
    }
}
