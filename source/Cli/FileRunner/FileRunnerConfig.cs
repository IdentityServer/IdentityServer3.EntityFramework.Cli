using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;

namespace IdentityServer3.EntityFramework.Cli.FileRunner
{
    public class FileRunnerConfig
    {
        public SectionConfig<Client> Clients { get; set; }
        public SectionConfig<Scope> Scopes { get; set; }
    }

    public class SectionConfig<T>
    {
        public string[] Remove { get; set; }
        public T[] Add { get; set; }
    }
}
