using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IdentityServer3.EntityFramework.Cli.FileRunner
{
    public class DataFileLoader
    {
        internal static string Load(string file)
        {
            if (!Path.IsPathRooted(file))
            {
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            }
            file = Path.GetFullPath(file);

            Console.WriteLine();
            Console.WriteLine(" Loading data from file:\r\n\t{0}", file);
            
            if (!File.Exists(file))
            {
                throw new InvalidOperationException("File does not exist");
            }
            
            var json = File.ReadAllText(file);
            return json;
        }
    }
}
