using TQ.SharedKernel;
using TQ.SharedKernel.Interfaces;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Core.Aggregates.UsersAggregate
{
    public class User : BaseEntity<IntId>, IAggregateRoot
    {
        /// <summary>
        /// id команды
        /// </summary>
        public IntId TeamId { get; private set; }
        /// <summary>
        /// имя
        /// </summary>
        public string FirstName { get; private set; }
        /// <summary>
        /// фамилия 
        /// </summary>
        public string LastName { get; private set; }
        /// <summary>
        /// код участник на браслете
        /// </summary>
        public int? Code { get; private set; }

        public User() { }
        public User(IntId teamId, string firstName, string lastName, int? code)
        {
            TeamId = teamId;
            FirstName = firstName;
            LastName = lastName;
            Code = code;
        }

        public void UpdateTeamId(IntId teamId)
        {
            TeamId = teamId;
        }
        public void UpdateFirstName(string firstName)
        {
            FirstName = firstName;
        }
        public void UpdateLastName(string lastName)
        {
            LastName = lastName;
        }
        public void UpdateCode(int? code)
        {
            Code = code;
        }
    }
}
