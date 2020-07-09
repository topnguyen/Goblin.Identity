using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityConfirmEmailModel : GoblinApiSubmitRequestModel
    {
        public string EmailConfirmToken { get; set; }
    }
}