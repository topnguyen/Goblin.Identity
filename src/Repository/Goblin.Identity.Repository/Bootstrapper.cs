using System.Threading;
using System.Threading.Tasks;
using Elect.Data.EF.Interfaces.DbContext;
using Elect.DI.Attributes;
using Goblin.Identity.Contract.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Goblin.Identity.Repository
{
    [ScopedDependency(ServiceType = typeof(IBootstrapper))]
    public class Bootstrapper : IBootstrapper
    {
        private readonly IDbContext _dbContext;

        public Bootstrapper(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task InitialAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task RebuildAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}