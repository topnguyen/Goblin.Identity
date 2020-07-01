using Goblin.Identity.Contract.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Identity.Repository
{
    public sealed partial class GoblinDbContext
    {
        public DbSet<SampleEntity> Samples { get; set; }
    }
}