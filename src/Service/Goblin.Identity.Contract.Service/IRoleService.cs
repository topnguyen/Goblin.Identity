using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Goblin.Identity.Share.Models.RoleModels;

namespace Goblin.Identity.Contract.Service
{
    public interface IRoleService
    {
        Task<List<string>> GetAllRolesAsync(CancellationToken cancellationToken = default);

        Task<GoblinIdentityRoleModel> UpsertAsync(GoblinIdentityUpsertRoleModel model, CancellationToken cancellationToken = default);
        
        Task<GoblinIdentityRoleModel> GetAsync(string name, CancellationToken cancellationToken = default);
        
        Task<List<string>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    }
}