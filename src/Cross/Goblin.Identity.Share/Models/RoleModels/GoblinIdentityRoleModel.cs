using System.Collections.Generic;

namespace Goblin.Identity.Share.Models.RoleModels
{
    public class GoblinIdentityRoleModel
    {
        public string Name { get; set; }
        
        public List<string> Permissions { get; set; }
    }
}