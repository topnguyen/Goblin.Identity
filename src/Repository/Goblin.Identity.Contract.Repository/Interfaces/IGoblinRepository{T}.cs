using Goblin.Identity.Contract.Repository.Models;

namespace Goblin.Identity.Contract.Repository.Interfaces
{
    public interface IGoblinRepository<T> : Elect.Data.EF.Interfaces.Repository.IBaseEntityRepository<T> where T : GoblinEntity, new()
    {
    }
}