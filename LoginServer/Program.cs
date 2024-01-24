using LoginServer.Communication;
using LoginServer.Database;
using LoginServer.Server;
using SharedLibrary.Util;

namespace LoginServer;

public static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    private static void Main()
    {
        var initServer = new InitServer();
        // Iniciar uma tarefa em segundo plano
        var backgroundTask = Task.Run(() => initServer.InitializeServer());

        while (true) 
        {            
            Console.ReadLine();
        }   
    }
}