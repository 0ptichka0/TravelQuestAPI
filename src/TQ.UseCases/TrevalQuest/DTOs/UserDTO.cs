namespace TQ.UseCases.TravelQuest.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// id команды
        /// </summary>
        public int TeamId { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// фамилия 
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// код участник на браслете
        /// </summary>
        public int? Code { get; set; }
    }
}
