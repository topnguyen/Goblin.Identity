using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Swagger.Attributes;
using Goblin.Core.Models;
using Goblin.Core.Web.Utils;
using Goblin.Identity.Contract.Service;
using Goblin.Identity.Share;
using Goblin.Identity.Share.Models.UserModels;
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
        ///     Get Paged User
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("User")]
        [HttpPost]
        [Route(GoblinIdentityEndpoints.GetPagedUser)]
        [SwaggerResponse(StatusCodes.Status200OK, "User Paged with Metadata", typeof(GoblinApiPagedMetaResponseModel<GoblinIdentityGetPagedUserModel, GoblinIdentityUserModel>))]
        public async Task<IActionResult> GetPaged([FromBody] GoblinIdentityGetPagedUserModel model, CancellationToken cancellationToken = default)
        {
            var userPagedModel = await _userService.GetPagedAsync(model, cancellationToken);

            var userPagedWithMetadataResponseModel = Url.GetGoblinApiPagedMetaResponseModel(model, userPagedModel);
            
            return Ok(userPagedWithMetadataResponseModel);
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
        [SwaggerResponse(StatusCodes.Status201Created, "User Saved", typeof(GoblinIdentityEmailConfirmationModel))]
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
        [Route(GoblinIdentityEndpoints.GetProfile)]
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
        public async Task<IActionResult> UpdateProfile([FromRoute] long id, [FromBody] GoblinIdentityUpdateProfileModel model, CancellationToken cancellationToken = default)
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
        public async Task<IActionResult> UpdateIdentity([FromRoute] long id, [FromBody] GoblinIdentityUpdateIdentityModel model, CancellationToken cancellationToken = default)
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
        
        /// <summary>
        ///     Generate Access Token
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Auth")]
        [HttpPost]
        [Route(GoblinIdentityEndpoints.GenerateAccessToken)]
        [SwaggerResponse(StatusCodes.Status200OK, "Access Token", typeof(string))]
        public async Task<IActionResult> GenerateAccessToken([FromBody] GoblinIdentityGenerateAccessTokenModel model, CancellationToken cancellationToken = default)
        {
            var accessToken = await _userService.GenerateAccessTokenAsync(model, cancellationToken);

            return Ok(accessToken);
        }
        
        /// <summary>
        ///     Get Profile by Access Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Auth")]
        [HttpGet]
        [Route(GoblinIdentityEndpoints.GetProfileByAccessToken)]
        [SwaggerResponse(StatusCodes.Status200OK, "User Profile Information", typeof(GoblinIdentityUserModel))]
        public async Task<IActionResult> GetProfileByAccessToken([FromQuery] string accessToken, CancellationToken cancellationToken = default)
        {
            var userModel = await _userService.GetProfileByAccessTokenAsync(accessToken, cancellationToken);

            return Ok(userModel);
        }
        
        /// <summary>
        ///     Request Reset Password
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Auth")]
        [HttpPost]
        [Route(GoblinIdentityEndpoints.RequestResetPassword)]
        [SwaggerResponse(StatusCodes.Status200OK, "Reset Password Token", typeof(GoblinIdentityResetPasswordTokenModel))]
        public async Task<IActionResult> RequestResetPassword([FromBody] GoblinIdentityRequestResetPasswordModel model, CancellationToken cancellationToken = default)
        {
            var resetPasswordToken = await _userService.RequestResetPasswordAsync(model, cancellationToken);

            return Ok(resetPasswordToken);
        }
        
        /// <summary>
        ///     Reset Password
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Auth")]
        [HttpPut]
        [Route(GoblinIdentityEndpoints.ResetPassword)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Update Password Successfuly")]
        public async Task<IActionResult> ResetPassword([FromBody] GoblinIdentityResetPasswordModel model, CancellationToken cancellationToken = default)
        {
            await _userService.ResetPasswordAsync(model, cancellationToken);

            return NoContent();
        }
        
        /// <summary>
        ///     Request Confirm Email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Auth")]
        [HttpPost]
        [Route(GoblinIdentityEndpoints.RequestConfirmEmail)]
        [SwaggerResponse(StatusCodes.Status200OK, "Confirm Email Token", typeof(GoblinIdentityEmailConfirmationModel))]
        public async Task<IActionResult> RequestConfirmEmail([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var emailConfirmationModel = await _userService.RequestConfirmEmailAsync(id, cancellationToken);

            return Ok(emailConfirmationModel);
        }


        /// <summary>
        ///     Confirm Email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ApiDocGroup("Auth")]
        [HttpPut]
        [Route(GoblinIdentityEndpoints.ConfirmEmail)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Email Confirmed")]
        public async Task<IActionResult> ConfirmEmail([FromRoute] long id, [FromBody] GoblinIdentityConfirmEmailModel model, CancellationToken cancellationToken = default)
        {
            await _userService.ConfirmEmail(id, model, cancellationToken);

            return NoContent();
        }

    }
}