using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Goblin.Identity.Repository
{
    public class GoblinDbContextFactory: IDesignTimeDbContextFactory<GoblinDbContext>
    {
        public GoblinDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GoblinDbContext>();

            GoblinDbContextFactoryHelper.Build(null, optionsBuilder);
            
            return new GoblinDbContext(optionsBuilder.Options);
        }
    }
}