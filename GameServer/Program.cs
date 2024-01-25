﻿
using GameServer.Communication;
using GameServer.Server;

namespace Program;

public static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    private static void Main()
    {
        Global.Server = new DataServer();
        Global.Server.UpdateUps += ups => Console.Title = $"Game Server @ {ups} Ups";
        Global.Server.InitializeServer();


        while (true)
        {
            Console.ReadLine();
        }
    }
}