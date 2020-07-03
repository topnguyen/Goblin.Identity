using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityGetPagedUserModel : GoblinApiPagedRequestModel
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}