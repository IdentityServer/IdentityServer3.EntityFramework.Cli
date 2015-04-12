using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer3.EntityFramework.Cli.FileRunner
{
    public class FileRunnerContext : RunContext
    {
        public string File { get; set; }

        public override void Run()
        {
            var json = DataFileLoader.Load(File);
            var config = JsonConvert.DeserializeObject<FileRunnerConfig>(json);

            if (config.Clients != null)
            {
                var r = new ClientRunner(this);
                r.Run(config.Clients);
            }

            if (config.Scopes != null)
            {
                var r = new ScopeRunner(this);
                r.Run(config.Scopes);
            }
        }
    }
}
