namespace Goblin.Identity.Contract.Repository.Models
{
    public class UserRoleEntity : GoblinEntity
    {
        public long UserId { get; set; }

        public UserEntity User { get; set; }
        
        public long RoleId { get; set; }

        public RoleEntity Role { get; set; }
    }
}