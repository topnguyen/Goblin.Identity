using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Swagger.Attributes;
using Goblin.Identity.Contract.Service;
using Goblin.Identity.Share;
using Goblin.Identity.Share.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Goblin.Identity.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        ///     Register User
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("User")]
        [HttpPost]
        [Route(GoblinIdentityEndpoints.RegisterUser)]
        [SwaggerResponse(StatusCodes.Status201Created, "Sample Saved", typeof(GoblinIdentityRegisterResponseModel))]
        public async Task<IActionResult> Register([FromBody] GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default)
        {
            var registerResponseModel = await _userService.RegisterAsync(model, cancellationToken);
            
            return Created(Url.Action("Get", new {registerResponseModel.Id}), registerResponseModel);
        }
        
        /// <summary>
        ///     Get Profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("User")]
        [HttpGet]
        [Route(GoblinIdentityEndpoints.GetUser)]
        [SwaggerResponse(StatusCodes.Status200OK, "User Profile Information", typeof(GoblinIdentityUserModel))]
        public async Task<IActionResult> Get([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var sampleModel = await _userService.GetAsync(id, cancellationToken);

            return Ok(sampleModel);
        }
        
        /// <summary>
        ///     Delete User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("User")]
        [HttpDelete]
        [Route(GoblinIdentityEndpoints.DeleteUser)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User Deleted")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            await _userService.DeleteAsync(id, cancellationToken);
            
            return NoContent();
        }
    }
}