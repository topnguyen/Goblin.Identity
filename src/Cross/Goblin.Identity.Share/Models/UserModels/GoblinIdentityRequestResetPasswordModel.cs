using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityRequestResetPasswordModel : GoblinApiSubmitRequestModel
    {
        public string Email { get; set; }
    }
}