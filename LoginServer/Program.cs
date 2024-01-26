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
        var Server = new DataServer();
        Server.UpdateUps += ups => Console.Title = $"Login Server @ {ups} Ups";
        Server.InitializeServer();

        while (true) 
        {            
            Console.ReadLine();
        }   
    }
}