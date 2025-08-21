namespace TQ.UseCases.TravelQuest.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// id забега
        /// </summary>
        public string RunId { get; set; }
        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// Название команды
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Код команды
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Територия
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// группа
        /// </summary>
        public string Group { get; set; }
    }
}
