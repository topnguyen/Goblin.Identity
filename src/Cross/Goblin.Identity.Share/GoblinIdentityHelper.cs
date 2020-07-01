using System;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Goblin.Core.Constants;
using Goblin.Core.Errors;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Share
{
    public static class GoblinIdentityHelper
    {
        public static string Domain { get; set; } = string.Empty;
        
        public static string AuthorizationKey { get; set; } = string.Empty;

        public static async Task<GoblinIdentitySampleModel> CreateAsync(GoblinIdentityCreateSampleModel model,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain;

                var fileModel = await endpoint
                    .AppendPathSegment(GoblinIdentityEndpoints.CreateSample)
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentitySampleModel>()
                    .ConfigureAwait(true);

                return fileModel;
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

        public static async Task<GoblinIdentitySampleModel> GetAsync(GoblinIdentityGetFileModel model,
            CancellationToken cancellationToken = default)
        {
            var endpoint = Domain.Replace("{id}", model.Id.ToString());

            var fileModel =
                await endpoint
                    .AppendPathSegment(GoblinIdentityEndpoints.GetSample)
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .SetQueryParam(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .GetJsonAsync<GoblinIdentitySampleModel>(cancellationToken: cancellationToken)
                    .ConfigureAwait(true);

            return fileModel;
        }

        public static async Task DeleteAsync(GoblinIdentityDeleteSampleModel model,
            CancellationToken cancellationToken = default)
        {
            var endpoint = Domain.Replace("{id}", model.Id.ToString());

            await endpoint
                .AppendPathSegment(GoblinIdentityEndpoints.DeleteSample)
                .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(true);
        }
    }
}