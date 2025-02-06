using DbUp;

namespace WorkLogger.Infrastructure;

public class Migrator
{
    public static void Migrate(string connectionString, string fullPathToScripts)
    {
        Environment.SetEnvironmentVariable("DOTNET_SYSTEM_GLOBALIZATION_INVARIANT", "0");
        AppContext.SetSwitch("System.Globalization.Invariant", false);
        
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsFromFileSystem(fullPathToScripts)
                .WithTransaction()
                .LogToConsole()
                .Build();

        try
        {
            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                return;
            }
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
    }
}