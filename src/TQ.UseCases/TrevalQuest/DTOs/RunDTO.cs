namespace TQ.UseCases.TravelQuest.DTOs
{
    public class RunDTO
    {
        public string Id { get; set; }
        /// <summary>
        /// Название команды
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Дата и время старта
        /// </summary>
        public DateTime RunStart { get; set; }
        /// <summary>
        /// Продолжительность соревнований (hh:mm:ss)
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string? Description { get; set; }
    }
}
