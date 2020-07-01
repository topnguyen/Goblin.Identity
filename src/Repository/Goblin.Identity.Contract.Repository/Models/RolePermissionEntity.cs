namespace Goblin.Identity.Contract.Repository.Models
{
    public class RolePermissionEntity : GoblinEntity
    {
        public long RoleId { get; set; }

        public RoleEntity Role { get; set; }
        
        public long PermissionId { get; set; }

        public PermissionEntity Permission { get; set; }
    }
}