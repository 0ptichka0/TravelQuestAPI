using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.SharedKernel;
using TQ.SharedKernel.Interfaces;

namespace TQ.Core.Aggregates.CPsAggregate
{
    public class CP : BaseEntity<CPId>, IAggregateRoot
    {
        /// <summary>
        /// Номер КП
        /// </summary>
        public int Number { get; private set; }
        /// <summary>
        /// Легенда места КП
        /// </summary>
        public string Legend { get; private set; }
        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; private set; }
        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; private set; }

        public CP() { }
        public CP(CPId id, int number, string legend, double latitude, double longitude)
        {
            Id = id;
            Number = number;
            Legend = legend;
            Latitude = latitude;
            Longitude = longitude;
        }

        public void UpdateId(CPId id)
        {
            Id = id;
        }
        public void UpdateNumber(int number)
        {
            Number = number;
        }
        public void UpdateLegend(string legend)
        {
            Legend = legend;
        }
        public void UpdateLatitude(double latitude)
        {
            Latitude = latitude;
        }
        public void UpdateLongitude(double longitude)
        {
            Longitude = longitude;
        }
    }
}
