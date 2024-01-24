using LoginServer.Communication;

namespace LoginServer.Server;

public class InitServer
{
    public void InitializeServer()
    {
        //Inicializações estáticas
        Global.InitLogs();

        Global.Server = new DataServer();
        Global.Server.UpdateUps += ups => Console.Title = $"Login Server @ {ups} Ups";
        Global.Server.InitServer();
    }
}