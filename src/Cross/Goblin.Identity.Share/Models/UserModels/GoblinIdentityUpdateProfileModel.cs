using System.Collections.Generic;
using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityUpdateProfileModel : GoblinApiRequestModel
    {
        // Basic Profile

        public string AvatarUrl { get; set; }

        public string FullName { get; set; }
        
        public string Bio { get; set; }
        
        // Social
        
        public string GithubId { get; set; }
        
        public string SkypeId { get; set; }
        
        public string FacebookId { get; set; }

        public string WebsiteUrl { get; set; }
        
        // Company
        
        public string CompanyName { get; set; }
        
        public string CompanyUrl { get; set; }
        
        // Role
        
        public bool IsUpdateRoles { get; set; } = false;

        public List<string> Roles { get; set; }
    }
}