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
        [SwaggerResponse(StatusCodes.Status201Created, "Sample Saved", typeof(GoblinIdentityEmailConfirmationModel))]
        public async Task<IActionResult> Register([FromBody] GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default)
        {
            var registerResponseModel = await _userService.RegisterAsync(model, cancellationToken);
            
            return Created(Url.Action("GetProfile", new {registerResponseModel.Id}), registerResponseModel);
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
        public async Task<IActionResult> GetProfile([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var userModel = await _userService.GetProfileAsync(id, cancellationToken);

            return Ok(userModel);
        }

        /// <summary>
        ///     Update Profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("User")]
        [HttpPut]
        [Route(GoblinIdentityEndpoints.UpdateProfile)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "User Profile Updated")]
        public async Task<IActionResult> UpdateProfile([FromRoute] long id, GoblinIdentityUpdateProfileModel model, CancellationToken cancellationToken = default)
        {
            await _userService.UpdateProfileAsync(id, model, cancellationToken);

            return NoContent();
        }
        
        /// <summary>
        ///     Update Identity
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("User")]
        [HttpPut]
        [Route(GoblinIdentityEndpoints.UpdateIdentity)]
        [SwaggerResponse(StatusCodes.Status200OK, "User Identity Updated")]
        public async Task<IActionResult> UpdateIdentity([FromRoute] long id, GoblinIdentityUpdateIdentityModel model, CancellationToken cancellationToken = default)
        {
            var emailConfirmationModel = await _userService.UpdateIdentityAsync(id, model, cancellationToken);

            return Ok(emailConfirmationModel);
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