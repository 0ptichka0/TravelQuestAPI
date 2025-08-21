namespace TQ.UseCases.TravelQuest.DTOs
{
    public class CPDTO
    {
        public string Id { get; set; }
        /// <summary>
        /// Номер КП ????????
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Легенда места КП
        /// </summary>
        public string Legend { get; set; }
        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; set; }
    }
}
