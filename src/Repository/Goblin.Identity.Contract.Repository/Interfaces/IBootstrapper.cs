using System.Threading;
using System.Threading.Tasks;

namespace Goblin.Identity.Contract.Repository.Interfaces
{
    public interface IBootstrapper
    {
        Task InitialAsync(CancellationToken cancellationToken = default);

        Task RebuildAsync(CancellationToken cancellationToken = default);
    }
}