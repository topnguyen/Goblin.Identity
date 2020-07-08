using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityResetPasswordModel : GoblinApiSubmitRequestModel
    {
        public string Email { get; set; }
        
        public string SetPasswordToken { get; set; }

        public string NewPassword { get; set; }
    }
}