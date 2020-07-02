using System;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityUserModel
    {
        public long Id { get; set; }
        
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

        // Identity

        public string UserName { get; set; }

        // Email

        public string Email { get; set; }

        public DateTimeOffset? EmailConfirmedTime { get; set; }

        /// <summary>
        ///     Remove token generated before specific time
        /// </summary>
        public DateTimeOffset RevokeTokenGeneratedBeforeTime { get; set; }
    }
}