namespace TQ.UseCases.TravelQuest.DTOs
{
    public class CPRunDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// id забега
        /// </summary>
        public string RunId { get; set; }
        /// <summary>
        /// id контрольных пунктов
        /// </summary>
        public string CPId { get; set; }
        /// <summary>
        /// баллов за КП
        /// </summary>
        public int Scores { get; set; }
    }
}
