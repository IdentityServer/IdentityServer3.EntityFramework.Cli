using IdentityServer3.EntityFramework.Cli.FileRunner;
using IdentityServer3.EntityFramework.Cli.PermissionsRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer3.EntityFramework.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    throw new UsageException();
                }

                var connectionString = ReadParam(args, "-connection") ?? ReadParam(args, "-c");
                var schema = ReadParam(args, "-schema") ?? ReadParam(args, "-s") ?? null;
                if (connectionString == null)
                {
                    throw new UsageException();
                }

                RunContext ctx = null;

                var file = ReadParam(args, "-file") ?? ReadParam(args, "-f");
                var list = ReadFlag(args, "-list") || ReadFlag(args, "-l");
                var revoke = ReadFlag(args, "-revoke") || ReadFlag(args, "-r");
                var subject = ReadParam(args, "-subject") ?? ReadParam(args, "-sub");
                var client = ReadParam(args, "-client") ?? ReadParam(args, "-cli");
                
                if (file != null)
                {
                    ctx = new FileRunnerContext
                    {
                        ConnectionString = connectionString,
                        Schema = schema,
                        File = file
                    };
                }
                else if (list || revoke)
                {
                    ctx = new PermissionsRunnerContext
                    {
                        ConnectionString = connectionString, 
                        Schema = schema,
                        Command = list ? "list" : "revoke",
                        Subject = subject,
                        Client = client
                    };
                }

                if (ctx == null)
                {
                    throw new UsageException();
                }

                Console.WriteLine();
                Console.Write("Using Connection String Name: {0}", connectionString);
                if (schema != null)
                {
                    Console.Write(" (with schema: {0})", schema);
                }
                Console.WriteLine();
                
                ctx.Run();

                Console.WriteLine();
                Console.WriteLine("Done.");
            }
            catch (UsageException)
            {
                Usage();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("Error ({1}): {0}", ex.Message, ex.GetType().Name);
            }
        }

        private static bool ReadFlag(string[] args, string flag)
        {
            var idx = Array.IndexOf(args, flag);
            return (idx >= 0);
        }

        private static string ReadParam(string[] args, string flag)
        {
            var idx = Array.IndexOf(args, flag);
            if (idx >= 0)
            {
                if (args.Length > idx + 1)
                {
                    return args[idx + 1];
                }
                else
                {
                    throw new UsageException();
                }
            }

            return null;
        }

        private static void Usage()
        {
            Console.WriteLine("IdSvr3EfCli : Configures an IdentityServer EF-based database.");
            Console.WriteLine();

            Console.WriteLine("Usage: ");
            Console.WriteLine("IdSvr3EfCli");
            Console.WriteLine(" Connection String\t[-connection | -c] connection_string_name");
            Console.WriteLine(" Optional Schema\t[-schema|-s database_schema]");
            Console.WriteLine(" Data file to load\t[-file|-f path_to_data_file_with_clients_and_scopes]");
            Console.WriteLine(" List permissions\t[-list|-l [-sub subject]]");
            Console.WriteLine(" Revoke permissions\t[-revoke|-r [-sub subject] [-cli client]]");

            Console.WriteLine();
        }
    }
}
