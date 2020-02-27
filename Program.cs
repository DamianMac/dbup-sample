using System;
using System.Reflection;
using System.Linq;
using DbUp;
using DbUp.Engine;
using DbUp.Helpers;

namespace dbup_sample
{
    class Program
    {
        static int Main(string[] args)
        {
            var connectionString =
                args.FirstOrDefault()
                ?? "Server=(local); Database=MyApp; Trusted_connection=true";


            var upgrader = GetMigrationUpgrader(connectionString);

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            upgrader = GetStoredProcedureDeployer(connectionString);
            result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        private static UpgradeEngine GetMigrationUpgrader(string connectionString)
        {
            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.Contains(("ChangeScripts")))
                    .LogToConsole()
                    .Build();
            return upgrader;
        }
        
        private static UpgradeEngine GetStoredProcedureDeployer(string connectionString)
        {
            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), s => s.Contains(("StoredProcedures")))
                    .LogToConsole()
                    .JournalTo(new NullJournal())
                    .Build();
            return upgrader;
        }
    }
}
