using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.SharedKernel;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Aggregates.CPsRunsAggregate
{
    public class CPRun : BaseEntity<IntId>, IAggregateRoot
    {
        /// <summary>
        /// id забега
        /// </summary>
        public RunId RunId { get; private set; }
        /// <summary>
        /// id контрольных пунктов
        /// </summary>
        public CPId CPId { get; private set; }
        /// <summary>
        /// баллов за КП
        /// </summary>
        public int Scores { get; private set; }

        public CPRun() { }
        public CPRun(RunId runId, CPId cPId, int scores)
        {
            RunId = runId;
            CPId = cPId;
            Scores = scores;
        }

        public void UpdateRunId(RunId runId)
        {
            RunId = runId;
        }
        public void UpdateCPId(CPId runId)
        {
            CPId = runId;
        }
        public void UpdateScores(int scores)
        {
            Scores = scores;
        }
    }
}
