using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models
{
    public class GoblinIdentityUpdateIdentityModel : GoblinApiRequestModel
    {
        public string CurrentPassword { get; set; }
        
        // Identity

        public string NewEmail { get; set; }

        public string NewUserName { get; set; }

        public string NewPassword { get; set; }
    }
}