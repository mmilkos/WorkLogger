using System.Reflection;
using DbUp;
using Microsoft.Data.SqlClient;

namespace WorkLogger.Infrastructure;

public class Migrator
{
    private const string scriptsFolder = "Scripts";
    public static void Migrate(string connectionString)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader =
            DeployChanges.To
                .SqlDatabase(connectionString)
                .WithScriptsFromFileSystem(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    scriptsFolder
                ))
                .WithTransaction()
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();
        
        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
    }
}