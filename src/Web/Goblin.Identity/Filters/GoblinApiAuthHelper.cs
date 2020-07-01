using System;
using Goblin.Core.Constants;
using Goblin.Core.Web.Utils;
using Goblin.Identity.Core;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Goblin.Identity.Filters
{
    public static class GoblinApiAuthHelper
    {
        public static bool IsAuthenticate(AuthorizationFilterContext context)
        {
            if (string.IsNullOrWhiteSpace(SystemSetting.Current.AuthorizationKey))
            {
                return true;
            }

            var authenticationKey = context.HttpContext.GetKey<string>(GoblinHeaderKeys.Authorization);

            var isAuthenticate = string.Equals(authenticationKey?.Trim(),
                SystemSetting.Current.AuthorizationKey?.Trim(), StringComparison.InvariantCultureIgnoreCase);

            return isAuthenticate;
        }

       
    }
}