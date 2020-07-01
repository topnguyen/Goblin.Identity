using System.Collections.Generic;

namespace Goblin.Identity.Contract.Repository.Models
{
    public class RoleEntity : GoblinEntity
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public virtual ICollection<RolePermissionEntity> RolePermissions { get; set; }
        
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }

    }
}