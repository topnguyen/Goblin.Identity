using System.Threading;
using System.Threading.Tasks;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Contract.Service
{
    public interface ISampleService
    {
        Task<GoblinIdentitySampleModel> CreateAsync(GoblinIdentityCreateSampleModel model, CancellationToken cancellationToken = default);
        
        Task<GoblinIdentitySampleModel> GetAsync(long id, CancellationToken cancellationToken = default);
        
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}