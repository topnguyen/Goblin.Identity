using System;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Contract.Repository.Models
{
    public class UserEntity : GoblinEntity
    {
        public GoblinIdentityUserType Type { get; set; } = GoblinIdentityUserType.Member;

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

        public string PasswordHash { get; set; }

        public DateTimeOffset PasswordLastUpdatedTime { get; set; }
        
        // Set Password

        public string SetPasswordToken { get; set; }

        public DateTimeOffset? SetPasswordTokenExpireTime { get; set; }
        
        // Email

        public string Email { get; set; }

        public DateTimeOffset? EmailConfirmedTime { get; set; }

        public string EmailConfirmToken { get; set; }

        public DateTimeOffset? EmailConfirmTokenExpireTime { get; set; } 
        
        /// <summary>
        ///     Remove token generated before specific time
        /// </summary>
        public DateTimeOffset RevokeTokenGeneratedBeforeTime { get; set; }
    }
}