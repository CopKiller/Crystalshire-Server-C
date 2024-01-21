using LoginServer.Communication;
using LoginServer.Database;

namespace LoginServer.Server;

public class InitServer
{
    public async Task InitServerAsync()
    {
        //Inicializações estáticas
        await Global.InitLogs();

        await DatabaseStartup.Configure();

        Global.Server = new DataServer();
        Global.Server.UpdateUps += ups => Console.Title = $"Login Server @ {ups} Ups";
        Global.Server.InitServer();
    }
}