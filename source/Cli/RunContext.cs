using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer3.EntityFramework.Cli
{
    public abstract class RunContext
    {
        public string ConnectionString { get; set; }
        public string Schema { get; set; }

        public abstract void Run();
    }
}
