using System;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Goblin.Core.Errors;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Share
{
    public static class GoblinIdentityHelper
    {
        public static string Domain { get; set; } = string.Empty;
        
        public static string AuthorizationKey { get; set; } = string.Empty;

        public static async Task<GoblinIdentityRegisterResponseModel> RegisterAsync(GoblinIdentityRegisterModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain;

                var registerResponseModel = await endpoint
                    .AppendPathSegment(GoblinIdentityEndpoints.RegisterUser)
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinIdentityRegisterResponseModel>()
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
    }
}