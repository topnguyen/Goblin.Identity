using System;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityEmailConfirmationModel
    {
        public long Id { get; set; }
        
        public string EmailConfirmToken { get; set; }

        public DateTimeOffset? EmailConfirmTokenExpireTime { get; set; }
    }
}