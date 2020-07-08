using System;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityResetPasswordTokenModel
    {
        public string SetPasswordToken { get; set; }

        public DateTimeOffset? SetPasswordTokenExpireTime { get; set; }
    }
}