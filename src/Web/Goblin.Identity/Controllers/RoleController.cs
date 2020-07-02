using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Swagger.Attributes;
using Goblin.Identity.Contract.Service;
using Goblin.Identity.Share;
using Goblin.Identity.Share.Models.RoleModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Goblin.Identity.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        ///     Create or Update Role
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Role")]
        [HttpPost]
        [Route(GoblinIdentityEndpoints.UpsertRole)]
        [SwaggerResponse(StatusCodes.Status200OK, "Role Upserted", typeof(GoblinIdentityRoleModel))]
        public async Task<IActionResult> UpsertRole([FromBody] GoblinIdentityUpsertRoleModel model, CancellationToken cancellationToken = default)
        {
            var roleModel = await _roleService.UpsertAsync(model, cancellationToken);
            
            return Created(Url.Action("GetRole", new {roleModel.Name}), roleModel);
        }
        
        /// <summary>
        ///     Get Role Detail
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Role")]
        [HttpGet]
        [Route(GoblinIdentityEndpoints.GetRole)]
        [SwaggerResponse(StatusCodes.Status200OK, "Role with Permissions Information", typeof(GoblinIdentityRoleModel))]
        public async Task<IActionResult> GetRole([FromRoute] string name, CancellationToken cancellationToken = default)
        {
            var roleModel = await _roleService.GetAsync(name, cancellationToken);

            return Ok(roleModel);
        }
        
        /// <summary>
        ///     Get All Role Name
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Role")]
        [HttpGet]
        [Route(GoblinIdentityEndpoints.GetAllRoles)]
        [SwaggerResponse(StatusCodes.Status200OK, "All Role Name", typeof(List<string>))]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken = default)
        {
            var allRoles = await _roleService.GetAllRolesAsync(cancellationToken);

            return Ok(allRoles);
        }
        
        /// <summary>
        ///     Get All Permission Name
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Role")]
        [HttpGet]
        [Route(GoblinIdentityEndpoints.GetAllPermissions)]
        [SwaggerResponse(StatusCodes.Status200OK, "All Role Name", typeof(List<string>))]
        public async Task<IActionResult> GetAllPermissions(CancellationToken cancellationToken = default)
        {
            var allRoles = await _roleService.GetAllPermissionsAsync(cancellationToken);

            return Ok(allRoles);
        }
    }
}