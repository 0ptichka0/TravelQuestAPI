namespace TQ.UseCases.TravelQuest.DTOs
{
    public class TeamResultDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// id команды
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// всего баллов
        /// </summary>
        public int TotalScore { get; set; }
        /// <summary>
        /// потраченное время (от начала забега)
        /// </summary>
        public TimeSpan ElapsedTime { get; set; }
        /// <summary>
        /// штраф
        /// </summary>
        public int Penalty { get; set; }
    }
}
