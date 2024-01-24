
using GameServer.Communication;

namespace GameServer.Network;

public class InitServer
{
    public void InitializeServer()
    {
        //Inicializações estáticas
        Global.InitLogs();

        Global.Server = new DataServer();
        Global.Server.UpdateUps += ups => Console.Title = $"Game Server @ {ups} Ups";
        Global.Server.InitServer();
    }
}