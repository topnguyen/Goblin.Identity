using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Goblin.Core.Constants;
using Goblin.Core.Errors;
using Goblin.Core.Models;
using Goblin.Identity.Share.Models.RoleModels;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Share
{
    public static class GoblinIdentityHelper
    {
        public static string Domain { get; set; } = string.Empty;

        public static string AuthorizationKey { get; set; } = string.Empty;

        public static async Task<GoblinApiPagedMetaResponseModel<GoblinIdentityGetPagedUserModel, GoblinIdentityUserModel>> GetPagedAsync(GoblinIdentityGetPagedUserModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .AppendPathSegment(GoblinIdentityEndpoints.GetPagedUser);

                var userPagedMetaResponse = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinApiPagedMetaResponseModel<GoblinIdentityGetPagedUserModel, GoblinIdentityUserModel>>()
                    .ConfigureAwait(true);

                return userPagedMetaResponse;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }

        public static async Task<GoblinIdentityEmailConfirmationModel> RegisterAsync(GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.RegisterUser);

                var emailConfirmModel = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityEmailConfirmationModel>()
                    .ConfigureAwait(true);

                return emailConfirmModel;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }

        public static async Task ConfirmEmailAsync(GoblinIdentityConfirmEmailModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.ConfirmEmail);

                await endpoint
                    .PutJsonAsync(model, cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }
        
        public static async Task<GoblinIdentityUserModel> GetProfileAsync(long id, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .AppendPathSegment(GoblinIdentityEndpoints.GetProfile.Replace("{id}", id.ToString()));
                

                var userModel = await endpoint
                    .GetJsonAsync<GoblinIdentityUserModel>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return userModel;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }
        
        public static async Task UpdateProfileAsync(long id, GoblinIdentityUpdateProfileModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.UpdateProfile.Replace("{id}", id.ToString()));
                

                await endpoint
                    .PutJsonAsync(model, cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }

        public static async Task<GoblinIdentityEmailConfirmationModel> UpdateIdentityAsync(long id, GoblinIdentityUpdateIdentityModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.UpdateIdentity.Replace("{id}", id.ToString()));

                var emailConfirmModel = await endpoint
                    .PutJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityEmailConfirmationModel>()
                    .ConfigureAwait(true);

                return emailConfirmModel;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }

        public static async Task DeleteAsync(long id, long? loggedInUserId, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, loggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.DeleteUser.Replace("{id}", id.ToString()));

              await endpoint
                    .DeleteAsync(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }
        
        public static async Task<string> GenerateAccessTokenAsync(GoblinIdentityGenerateAccessTokenModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.GenerateAccessToken);

                var accessToken = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveString()
                    .ConfigureAwait(true);

                return accessToken;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }
        
        public static async Task<GoblinIdentityUserModel> GetProfileByAccessTokenAsync(string accessToken, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .AppendPathSegment(GoblinIdentityEndpoints.GetProfileByAccessToken)
                    .SetQueryParam("accessToken", accessToken);
                

                var userModel = await endpoint
                    .GetJsonAsync<GoblinIdentityUserModel>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return userModel;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }

        public static async Task<GoblinIdentityRoleModel> UpsertRoleAsync(GoblinIdentityUpsertRoleModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.UpsertRole);

                var roleModel = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityRoleModel>()
                    .ConfigureAwait(true);

                return roleModel;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }
        
        public static async Task<GoblinIdentityRoleModel> GetRoleAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .AppendPathSegment(GoblinIdentityEndpoints.GetRole.Replace("name", name));

                var roleModel = await endpoint
                    .GetJsonAsync<GoblinIdentityRoleModel>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return roleModel;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }
        
        public static async Task<List<string>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .AppendPathSegment(GoblinIdentityEndpoints.GetAllRoles);

                var roleNames = await endpoint
                    .GetJsonAsync<List<string>>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return roleNames;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }
        
        public static async Task<List<string>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .AppendPathSegment(GoblinIdentityEndpoints.GetAllPermissions);

                var permissionNames = await endpoint
                    .GetJsonAsync<List<string>>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

                return permissionNames;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }
    }
}