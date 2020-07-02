using System.Collections.Generic;
using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models.RoleModels
{
    public class GoblinIdentityUpsertRoleModel : GoblinApiRequestModel
    {
        public string Name { get; set; }
        
        public List<string> Permissions { get; set; }
    }
}