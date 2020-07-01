using System.ComponentModel.DataAnnotations;

namespace Goblin.Identity.Share.Models
{
    public enum GoblinIdentityUserType
    {
        [Display(Name = "Admin")]
        Admin = -1,

        [Display(Name = "Manager")]
        Manager = 1000,
        
        [Display(Name = "Member")]
        Member = 2000
    }
}