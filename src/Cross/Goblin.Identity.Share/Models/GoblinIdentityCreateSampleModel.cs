using System.ComponentModel.DataAnnotations;
using Goblin.Core.Models;

namespace Goblin.Identity.Share.Models
{
    public class GoblinIdentityCreateSampleModel : GoblinApiRequestModel
    {
        [Required]
        public string SampleData { get; set; }
    }
}