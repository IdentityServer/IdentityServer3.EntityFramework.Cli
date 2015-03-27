using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer3.EntityFramework.Cli
{
    public class RunContext
    {
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public string File { get; set; }
    }
}
