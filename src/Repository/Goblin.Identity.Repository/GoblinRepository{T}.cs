using Elect.DI.Attributes;
using Goblin.Identity.Contract.Repository.Interfaces;
using Goblin.Identity.Contract.Repository.Models;

namespace Goblin.Identity.Repository
{
    [ScopedDependency(ServiceType = typeof(IGoblinRepository<>))]
    public class GoblinRepository<T> : Elect.Data.EF.Services.Repository.BaseEntityRepository<T>, IGoblinRepository<T> where T : GoblinEntity, new()
    {
        public GoblinRepository(Elect.Data.EF.Interfaces.DbContext.IDbContext dbContext) : base(dbContext)
        {
        }
    }
}