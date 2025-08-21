namespace TQ.UseCases.TravelQuest.DTOs
{
    public class CPVisitDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// id команды
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// id КП
        /// </summary>
        public string CPId { get; set; }
        /// <summary>
        /// Подходит ли точка
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// Время визита (от начала забега)
        /// </summary>
        public TimeSpan VisitTime { get; set; }
    }
}
