using TQ.SharedKernel;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Aggregates.TeamResultsAggregate
{
    public class TeamResult : BaseEntity<IntId>, IAggregateRoot
    {
        /// <summary>
        /// id команды
        /// </summary>
        public IntId TeamId { get; private set; }
        /// <summary>
        /// всего баллов
        /// </summary>
        public int TotalScore { get; private set; }
        /// <summary>
        /// потраченное время (от начала забега)
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }
        /// <summary>
        /// штраф
        /// </summary>
        public int Penalty { get; private set; }

        public TeamResult() { }
        public TeamResult(IntId teamId, int totalScore, TimeSpan elapsedTime, int penalty)
        {
            TeamId = teamId;
            TotalScore = totalScore;
            ElapsedTime = elapsedTime;
            Penalty = penalty;
        }

        public void UpdateTeamId(IntId teamId)
        {
            TeamId = teamId;
        }
        public void UpdateTotalScore(int totalScore)
        {
            TotalScore = totalScore;
        }
        public void UpdateElapsedTime(TimeSpan elapsedTime)
        {
            ElapsedTime = elapsedTime;
        }
        public void UpdatePenalty(int penalty)
        {
            Penalty = penalty;
        }
    }
}
