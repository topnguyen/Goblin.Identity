using System;
using System.Threading;
using System.Threading.Tasks;
using Elect.Web.Models;
using Flurl.Http;
using Goblin.Core.Constants;
using Goblin.Core.Errors;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Share
{
    public static class GoblinIdentityHelper
    {
        public static string Domain { get; set; } = string.Empty;

        public static string AuthorizationKey { get; set; } = string.Empty;

        public static async Task<GoblinIdentityEmailConfirmationModel> RegisterAsync(GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .AppendPathSegment(GoblinIdentityEndpoints.RegisterUser);

                var registerResponseModel = await endpoint
                    .WithHeader(HeaderKey.Authorization, AuthorizationKey)
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityEmailConfirmationModel>()
                    .ConfigureAwait(true);

                return registerResponseModel;
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
                    .WithHeader(HeaderKey.Authorization, AuthorizationKey)
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
    }
}