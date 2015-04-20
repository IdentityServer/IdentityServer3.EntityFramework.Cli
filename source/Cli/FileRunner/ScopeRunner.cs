using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework;

namespace IdentityServer3.EntityFramework.Cli.FileRunner
{
    class ScopeRunner
    {
        private RunContext context;
        public ScopeRunner(RunContext ctx)
        {
            this.context = ctx;
        }
        
        internal void Run(SectionConfig<Scope> config)
        {
            Console.WriteLine();
            Console.WriteLine("Running scope configuration");
            Remove(config.Remove);
            Add(config.Add);
        }
        
        private void Add(Scope[] scopes)
        {
            if (scopes != null && scopes.Any())
            {
                Console.WriteLine();
                Console.WriteLine(" Adding Scopes");
                foreach (var scope in scopes)
                {
                    Add(scope);
                }
            }
        }
        
        private void Add(Scope scope)
        {
            Console.Write("\t{0}: ", scope.Name);

            using (var db = CreateContext())
            {
                var s = db.Scopes.SingleOrDefault(x => x.Name == scope.Name);
                if (s != null)
                {
                    Console.WriteLine("error: already exists.");
                    return;
                }

                var entity = scope.ToEntity();
                db.Scopes.Add(entity);
                try
                {
                    db.SaveChanges();
                    Console.WriteLine("success");
                }
                catch(Exception ex)
                {
                    Console.WriteLine("error: {0}", ex.Message);
                }
            }
        }

        private void Remove(string[] scopes)
        {
            if (scopes != null && scopes.Any())
            {
                Console.WriteLine();
                Console.WriteLine(" Removing Scopes");
                foreach (var scope in scopes)
                {
                    Remove(scope);
                }
            }
        }
        
        private void Remove(string scope)
        {
            Console.Write("\t{0}: ", scope);
            using(var db = CreateContext())
            {
                var s = db.Scopes.SingleOrDefault(x => x.Name == scope);
                if (s == null)
                {
                    Console.WriteLine("not found");
                }
                else
                {
                    db.Scopes.Remove(s);
                    db.SaveChanges();
                    Console.WriteLine("success");
                }
            }
        }

        ScopeConfigurationDbContext CreateContext()
        {
            return new ScopeConfigurationDbContext(context.ConnectionString, context.Schema);
        }
    }
}
