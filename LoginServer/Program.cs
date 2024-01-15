using System;
using LoginServer.Communication;
using LoginServer.Server;
using SharedLibrary.Util;

namespace LoginServer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var initServer = new InitServer();
            // Iniciar uma tarefa em segundo plano
            Task backgroundTask = Task.Run(() => initServer.InitServerAsync());

            while (true)
            {
                var response = Console.ReadLine();
                if (response != null)
                {
                    Global.WriteLog(LogType.Player, $"Comando recebido {response}",ConsoleColor.Blue);
                }
            }
        }
    }
}