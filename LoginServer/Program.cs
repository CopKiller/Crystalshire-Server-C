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
        Global.Server = new DataServer();
        Global.Server.UpdateUps += ups => Console.Title = $"Login Server @ {ups} Ups";
        Global.Server.InitializeServer();

        while (true) 
        {            
            Console.ReadLine();
        }   
    }
}