using Newtonsoft.Json;
using IdentityServer3.EntityFramework.Serialization;

namespace IdentityServer3.EntityFramework.Cli.FileRunner
{
    public class FileRunnerContext : RunContext
    {
        public string File { get; set; }

        public override void Run()
        {
            var json = DataFileLoader.Load(File);
            var config = JsonConvert.DeserializeObject<FileRunnerConfig>(json, new ClaimConverter());

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
