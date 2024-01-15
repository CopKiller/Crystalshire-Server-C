

using LoginServer.Communication;

namespace LoginServer.Server
{
    public class InitServer
    {
        public async Task InitServerAsync()
        {
            //Inicializações estáticas
            await Global.InitLogs();

            Global.Server = new DataServer();
            Global.Server.UpdateUps += ups => Console.Title = $"Event Server @ {ups} Ups";
            Global.Server.InitServer();
        }
    }
}
