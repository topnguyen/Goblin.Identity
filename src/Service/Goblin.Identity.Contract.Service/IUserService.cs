using System.Threading;
using System.Threading.Tasks;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Contract.Service
{
    public interface IUserService
    {
        Task<GoblinIdentityRegisterResponseModel> RegisterAsync(GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default);
        
        Task<GoblinIdentityUserModel> GetAsync(long id, CancellationToken cancellationToken = default);
        
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}