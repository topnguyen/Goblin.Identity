namespace Goblin.Identity.Share.Models.UserModels
{
    public class GoblinIdentityGetListUserModel
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public GetListUserSortBy SortBy { get; set; } = GetListUserSortBy.LastUpdatedTime;
    }
}