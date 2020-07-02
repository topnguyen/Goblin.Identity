using System.Threading;
using System.Threading.Tasks;
using Goblin.Identity.Share.Models;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Contract.Service
{
    public interface IUserService
    {
        Task<GoblinIdentityEmailConfirmationModel> RegisterAsync(GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default);
        
        Task<GoblinIdentityUserModel> GetProfileAsync(long id, CancellationToken cancellationToken = default);
        
        Task UpdateProfileAsync(long id, GoblinIdentityUpdateProfileModel model, CancellationToken cancellationToken = default);
        
        Task<GoblinIdentityEmailConfirmationModel> UpdateIdentityAsync(long id, GoblinIdentityUpdateIdentityModel model, CancellationToken cancellationToken = default);
        
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);

        Task<string> GenerateAccessTokenAsync(GoblinIdentityGenerateAccessTokenModel model, CancellationToken cancellationToken = default);
        
        Task<GoblinIdentityUserModel> GetProfileByAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default);
    }
}