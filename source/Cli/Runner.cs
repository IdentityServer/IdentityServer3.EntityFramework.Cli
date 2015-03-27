using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace IdentityServer3.EntityFramework.Cli
{
    public class Runner
    {
        public static void Run(RunContext ctx)
        {
            var json = DataFileLoader.Load(ctx.File);
            var config = JsonConvert.DeserializeObject<RunnerConfig>(json);
            
            if (config.Clients != null)
            {
                var r = new ClientRunner(ctx);
                r.Run(config.Clients);
            }

            if (config.Scopes != null)
            {
                var r = new ScopeRunner(ctx);
                r.Run(config.Scopes);
            }
        }
    }
}
