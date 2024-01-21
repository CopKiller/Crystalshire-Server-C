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

    enum MenuDb
    {
        AdicionarConta = 1,
        RealizarLogin = 2,
        ExcluirConta = 3
    }
    private static void Main()
    {
        var initServer = new InitServer();
        // Iniciar uma tarefa em segundo plano
        var backgroundTask = Task.Run(() => initServer.InitServerAsync());

        while (true) 
        {            
            Console.ReadLine();
        }   
    }
}