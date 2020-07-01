using Elect.Core.ConfigUtils;
using Goblin.Core.Web.Setup;
using Goblin.Identity.Core.Validators;
using Goblin.Identity.Core;
using Goblin.Identity.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Goblin.Identity
{
    public class Startup : BaseApiStartup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
            RegisterValidators.Add(typeof(IValidator));

            BeforeConfigureServices = services =>
            {
                // Setting

                SystemSetting.Current = Configuration.GetSection<SystemSetting>("Setting");
                
                // Database

                services.AddGoblinDbContext();
            };
        }
    }
}