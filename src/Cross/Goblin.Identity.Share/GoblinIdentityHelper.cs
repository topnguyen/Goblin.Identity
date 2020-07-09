using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Goblin.Core.Constants;
using Goblin.Core.Models;
using Goblin.Core.Settings;
using Goblin.Identity.Share.Models.RoleModels;
using Goblin.Identity.Share.Models.UserModels;
using Goblin.Identity.Share.Validators.RoleValidators;
using Goblin.Identity.Share.Validators.UserValidators;

namespace Goblin.Identity.Share
{
    public static class GoblinIdentityHelper
    {
        public static string Domain { get; set; } = string.Empty;

        public static string AuthorizationKey { get; set; } = string.Empty;

        public static readonly ISerializer JsonSerializer = new NewtonsoftJsonSerializer(GoblinJsonSetting.JsonSerializerSettings);

        private static IFlurlRequest GetRequest(long? loggedInUserId)
        {
            var request = Domain.WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey);

            if (loggedInUserId != null)
            {
                request = request.WithHeader(GoblinHeaderKeys.UserId, loggedInUserId);
            }

            request = request.ConfigureRequest(x =>
            {
                x.JsonSerializer = JsonSerializer;
            });

            return request;
        }

        public static async Task<GoblinApiPagedMetaResponseModel<GoblinIdentityGetPagedUserModel, GoblinIdentityUserModel>> GetPagedAsync(GoblinIdentityGetPagedUserModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = GetRequest(null).AppendPathSegment(GoblinIdentityEndpoints.GetPagedUser);

                var userPagedMetaResponse = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinApiPagedMetaResponseModel<GoblinIdentityGetPagedUserModel, GoblinIdentityUserModel>>()
                    .ConfigureAwait(true);

                return userPagedMetaResponse;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task<GoblinIdentityEmailConfirmationModel> RegisterAsync(GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinIdentityRegisterModelValidator, GoblinIdentityRegisterModel>(model);

            try
            {
                var endpoint = GetRequest(model.LoggedInUserId).AppendPathSegment(GoblinIdentityEndpoints.RegisterUser);

                var emailConfirmModel = await endpoint
                    .ConfigureRequest(x => { x.JsonSerializer = JsonSerializer; })
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityEmailConfirmationModel>()
                    .ConfigureAwait(true);

                return emailConfirmModel;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }
        
        public static async Task<GoblinIdentityUserModel> GetProfileAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = GetRequest(null).AppendPathSegment(GoblinIdentityEndpoints.GetProfile.Replace("{id}", id.ToString()));

                var userModel = await endpoint
                    .GetJsonAsync<GoblinIdentityUserModel>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return userModel;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task UpdateProfileAsync(long id, GoblinIdentityUpdateProfileModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinIdentityUpdateProfileModelValidator, GoblinIdentityUpdateProfileModel>(model);

            try
            {
                var endpoint =  GetRequest(model.LoggedInUserId).AppendPathSegment(GoblinIdentityEndpoints.UpdateProfile.Replace("{id}", id.ToString()));

                await endpoint
                    .PutJsonAsync(model, cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);
            }
        }

        public static async Task<GoblinIdentityEmailConfirmationModel> UpdateIdentityAsync(long id, GoblinIdentityUpdateIdentityModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinIdentityUpdateIdentityModelValidator, GoblinIdentityUpdateIdentityModel>(model);

            try
            {
                var endpoint =  GetRequest(model.LoggedInUserId).AppendPathSegment(GoblinIdentityEndpoints.UpdateIdentity.Replace("{id}", id.ToString()));

                var emailConfirmModel = await endpoint
                    .PutJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityEmailConfirmationModel>()
                    .ConfigureAwait(true);

                return emailConfirmModel;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task DeleteAsync(long id, long? loggedInUserId, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint =  GetRequest(loggedInUserId).AppendPathSegment(GoblinIdentityEndpoints.DeleteUser.Replace("{id}", id.ToString()));

                await endpoint
                    .DeleteAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);
            }
        }

        public static async Task<string> GenerateAccessTokenAsync(GoblinIdentityGenerateAccessTokenModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinIdentityGenerateAccessTokenModelValidator, GoblinIdentityGenerateAccessTokenModel>(model);

            try
            {
                var endpoint = GetRequest(model.LoggedInUserId).AppendPathSegment(GoblinIdentityEndpoints.GenerateAccessToken);

                var accessToken = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveString()
                    .ConfigureAwait(true);

                accessToken = accessToken?.Trim().Trim('"');
                
                return accessToken;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task<GoblinIdentityUserModel> GetProfileByAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = GetRequest(null)
                    .AppendPathSegment(GoblinIdentityEndpoints.GetProfileByAccessToken)
                    .SetQueryParam("accessToken", accessToken);


                var userModel = await endpoint
                    .GetJsonAsync<GoblinIdentityUserModel>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return userModel;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task<GoblinIdentityResetPasswordTokenModel> RequestResetPasswordAsync(GoblinIdentityRequestResetPasswordModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinIdentityRequestResetPasswordModelValidator, GoblinIdentityRequestResetPasswordModel>(model);

            try
            {
                var endpoint = GetRequest(model.LoggedInUserId).AppendPathSegment(GoblinIdentityEndpoints.RequestResetPassword);

                var resetPasswordToken = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityResetPasswordTokenModel>()
                    .ConfigureAwait(true);

                return resetPasswordToken;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task ResetPasswordAsync(GoblinIdentityResetPasswordModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinIdentityResetPasswordModelValidator, GoblinIdentityResetPasswordModel>(model);

            try
            {
                var endpoint = GetRequest(model.LoggedInUserId).AppendPathSegment(GoblinIdentityEndpoints.ResetPassword);

                await endpoint
                    .PutJsonAsync(model, cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);
            }
        }

        public static async Task<GoblinIdentityEmailConfirmationModel> RequestConfirmEmailAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = GetRequest(id).AppendPathSegment(GoblinIdentityEndpoints.RequestConfirmEmail.Replace("{id}", id.ToString()));

                var emailConfirmationModel = await endpoint
                    .PostJsonAsync(null, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityEmailConfirmationModel>()
                    .ConfigureAwait(true);

                return emailConfirmationModel;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task ConfirmEmailAsync(long id, GoblinIdentityConfirmEmailModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinIdentityConfirmEmailModelValidator, GoblinIdentityConfirmEmailModel>(model);

            try
            {
                var endpoint = GetRequest(model.LoggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.ConfirmEmail.Replace("{id}", id.ToString()));

                await endpoint
                    .PutJsonAsync(model, cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);
            }
        }

        public static async Task<GoblinIdentityRoleModel> UpsertRoleAsync(GoblinIdentityUpsertRoleModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinIdentityUpsertRoleModelValidator, GoblinIdentityUpsertRoleModel>(model);

            try
            {
                var endpoint = GetRequest(model.LoggedInUserId).AppendPathSegment(GoblinIdentityEndpoints.UpsertRole);

                var roleModel = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityRoleModel>()
                    .ConfigureAwait(true);

                return roleModel;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task<GoblinIdentityRoleModel> GetRoleAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = GetRequest(null).AppendPathSegment(GoblinIdentityEndpoints.GetRole.Replace("name", name));

                var roleModel = await endpoint
                    .GetJsonAsync<GoblinIdentityRoleModel>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return roleModel;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task<List<string>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = GetRequest(null).AppendPathSegment(GoblinIdentityEndpoints.GetAllRoles);

                var roleNames = await endpoint
                    .GetJsonAsync<List<string>>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return roleNames;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task<List<string>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = GetRequest(null).AppendPathSegment(GoblinIdentityEndpoints.GetAllPermissions);

                var permissionNames = await endpoint
                    .GetJsonAsync<List<string>>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return permissionNames;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }
    }
}