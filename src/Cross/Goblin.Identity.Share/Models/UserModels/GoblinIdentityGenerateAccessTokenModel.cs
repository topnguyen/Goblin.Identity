using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityGenerateAccessTokenModel : GoblinApiRequestModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

    }
}