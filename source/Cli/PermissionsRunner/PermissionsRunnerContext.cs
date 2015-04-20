using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.EntityFramework;

namespace IdentityServer3.EntityFramework.Cli.PermissionsRunner
{
    class PermissionsRunnerContext : RunContext
    {
        public string Command { get; set; }
        public string Subject { get; set; }
        public string Client { get; set; }

        public override void Run()
        {
            if (Command == "list" || Command == "l")
            {
                List();
            }
            if (Command == "revoke" || Command == "r")
            {
                Revoke();
            }
        }

        private void List()
        {
            using (var db = GetOperationalDbContext())
            {
                if (!String.IsNullOrWhiteSpace(Subject))
                {
                    Console.WriteLine("Listing client permissions for subject: {0}", Subject);

                    var consents =
                        from c in db.Consents
                        where c.Subject == this.Subject
                        select c.ClientId;
                    var tokens =
                        from t in db.Tokens
                        where t.SubjectId == Subject
                        select t.ClientId;

                    var clients = consents.Union(tokens).Distinct();
                    if (!clients.Any())
                    {
                        Console.WriteLine(" None found.");
                    }
                    foreach (var client in clients)
                    {
                        Console.WriteLine(" {0}", client);
                    }
                }
                else if (!String.IsNullOrWhiteSpace(Client))
                {
                    Console.WriteLine("Listing all subjects for client: {0}", Client);

                    var consents =
                        from c in db.Consents
                        where c.ClientId == Client
                        select c.Subject;
                    var tokens =
                        from t in db.Tokens
                        where t.ClientId == Client
                        select t.SubjectId;

                    var subs = consents.Union(tokens).Distinct();
                    if (!subs.Any())
                    {
                        Console.WriteLine(" None found.");
                    }
                    foreach (var sub in subs)
                    {
                        Console.WriteLine(" {0}", sub);
                    }
                }
                else
                {
                    Console.WriteLine("Listing all subjects and clients with permissions");

                    var consents =
                        from c in db.Consents
                        select new { Subject=c.Subject, Client=c.ClientId };
                    var tokens =
                        from t in db.Tokens
                        select new { Subject=t.SubjectId, Client=t.ClientId };

                    var results = consents.Union(tokens).Distinct();
                    if (!results.Any())
                    {
                        Console.WriteLine(" None found.");
                    }
                    foreach (var result in results)
                    {
                        Console.WriteLine(" {0}, {1}", result.Subject, result.Client);
                    }
                }
            }
        }

        private void Revoke()
        {
            if (String.IsNullOrWhiteSpace(Subject) && String.IsNullOrWhiteSpace(Client))
            {
                Console.WriteLine("Revoking all permissions for all clients");
            }
            else if (!String.IsNullOrWhiteSpace(Subject) && !String.IsNullOrWhiteSpace(Client))
            {
                Console.WriteLine("Revoking permissions for subject: {0} from client: {1}", Subject, Client);
            }
            else if (!String.IsNullOrWhiteSpace(Subject))
            {
                Console.WriteLine("Revoking all client permissions for subject: {0}", Subject);
            }
            else if (!String.IsNullOrWhiteSpace(Client))
            {
                Console.WriteLine("Revoking all permissions for client: {0}", Client);
            }

            using (var db = GetOperationalDbContext())
            {
                var consents =
                    from c in db.Consents
                    select c;
                if (!String.IsNullOrWhiteSpace(Subject))
                {
                    consents = consents.Where(x => x.Subject == Subject);
                }
                if (!String.IsNullOrWhiteSpace(Client))
                {
                    consents = consents.Where(x => x.ClientId == Client);
                }

                var tokens =
                    from t in db.Tokens
                    select t;
                if (!String.IsNullOrWhiteSpace(Subject))
                {
                    tokens = tokens.Where(x => x.SubjectId == Subject);
                }
                if (!String.IsNullOrWhiteSpace(Client))
                {
                    tokens = tokens.Where(x => x.ClientId == Client);
                }

                var any = consents.Any() || tokens.Any();
                
                db.Consents.RemoveRange(consents);
                db.Tokens.RemoveRange(tokens);
                db.SaveChanges();

                if (any)
                {
                    Console.WriteLine(" Permissions revoked.");
                }
                else
                {
                    Console.WriteLine(" None found.");
                }
            }
        }

        OperationalDbContext GetOperationalDbContext()
        {
            return new OperationalDbContext(this.ConnectionString, this.Schema);
        }
    }
}
