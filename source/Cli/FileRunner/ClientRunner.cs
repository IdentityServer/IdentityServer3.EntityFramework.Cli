using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework;

namespace IdentityServer3.EntityFramework.Cli.FileRunner
{
    class ClientRunner
    {
        private RunContext context;

        public ClientRunner(RunContext ctx)
        {
            this.context = ctx;
        }

        internal void Run(SectionConfig<Client> config)
        {
            Console.WriteLine();
            Console.WriteLine("Running client configuration");
            Remove(config.Remove);
            Add(config.Add);
        }

        private void Add(Client[] clients)
        {
            if (clients != null && clients.Any())
            {
                Console.WriteLine();
                Console.WriteLine(" Adding Clients");
                foreach (var client in clients)
                {
                    Add(client);
                }
            }
        }
        
        private void Add(Client client)
        {
            Console.Write("\t{0}: ", client.ClientId);

            using (var db = CreateContext())
            {
                var c = db.Clients.SingleOrDefault(x => x.ClientId == client.ClientId);
                if (c != null)
                {
                    Console.WriteLine("error: already exists.");
                    return;
                }

                HashPasswords(client);

                var entity = client.ToEntity();
                db.Clients.Add(entity);
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

        private void HashPasswords(Client client)
        {
            if (client.ClientSecrets != null)
            {
                foreach(var secret in client.ClientSecrets)
                {
                    if (secret.Value.StartsWith("sha256:"))
                    {
                        secret.Value = secret.Value.Split(':')[1].Sha256();
                    }
                    if (secret.Value.StartsWith("sha512:"))
                    {
                        secret.Value = secret.Value.Split(':')[1].Sha512();
                    }
                }
            }
        }

        private void Remove(string[] clients)
        {
            if (clients != null && clients.Any())
            {
                Console.WriteLine();
                Console.WriteLine(" Removing Clients");
                foreach (var client in clients)
                {
                    Remove(client);
                }
            }
        }
        
        private void Remove(string client)
        {
            Console.Write("\t{0}: ", client);
            using(var db = CreateContext())
            {
                var c = db.Clients.SingleOrDefault(x => x.ClientId == client);
                if (c == null)
                {
                    Console.WriteLine("not found");
                }
                else
                {
                    db.Clients.Remove(c);
                    db.SaveChanges();
                    Console.WriteLine("success");
                }
            }
        }

        ClientConfigurationDbContext CreateContext()
        {
            return new ClientConfigurationDbContext(context.ConnectionString, context.Schema);
        }
    }
}
