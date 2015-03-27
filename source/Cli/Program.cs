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

                var file = ReadFlag(args, "-file") ?? ReadFlag(args, "-f");
                var connectionString = ReadFlag(args, "-connection") ?? ReadFlag(args, "-c");
                var schema = ReadFlag(args, "-schema") ?? ReadFlag(args, "-s") ?? null;

                if (file == null || connectionString == null)
                {
                    throw new UsageException();
                }

                Console.WriteLine();
                Console.WriteLine("Using Connection String Name: {0}", connectionString);

                Runner.Run(new RunContext
                {
                    ConnectionString = connectionString,
                    Schema = schema,
                    File = file
                });

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

            Console.ReadLine();
        }

        private static string ReadFlag(string[] args, string flag)
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
            Console.WriteLine("IdSvr3EfCli : Configure IdentityServer database with clients and scopes");
            Console.WriteLine();

            Console.WriteLine("Usage: ");
            Console.WriteLine("IdSvr3EfCli\t[-connection | -c] connection_string_name");
            Console.WriteLine("\t\t[-file | -f] path_to_json_data_file");
            Console.WriteLine("\t\t[-schema database_schema | -s database_schema]");

            Console.WriteLine();
        }
    }
}
