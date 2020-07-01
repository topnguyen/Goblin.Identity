using System;

namespace Goblin.Identity.Share.Models
{
    public class GoblinIdentityRegisterResponseModel
    {
        public long Id { get; set; }
        
        public string EmailConfirmToken { get; set; }

        public DateTimeOffset? EmailConfirmTokenExpireTime { get; set; }
    }
}