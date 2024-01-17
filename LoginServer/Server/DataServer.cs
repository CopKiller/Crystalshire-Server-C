using LoginServer.Communication;
using LoginServer.Network;

namespace LoginServer.Server;

public class DataServer
{
    private int count;
    private TcpServer Server;

    private int tick;
    public Action<int> UpdateUps;
    private int ups;
    public bool ServerRunning { get; set; } = true;

    public async void InitServer()
    {
        Server = new TcpServer(Constants.Port);

        await Server.InitServer();

        OpCode.InitOpCode();

        while (true)
            if (Server != null)
            {
                ServerLoop();

                Thread.Sleep(1);

                if (!ServerRunning)
                {
                    StopServer();
                    Environment.Exit(0);
                }
            }
    }

    public void ServerLoop()
    {
        Server.AcceptClient();

        ReceiveSocketData();

        CountUps();
    }

    public void StopServer()
    {
        Server.Stop();
        Connection.Connections.Clear();
    }

    private void CountUps()
    {
        if (Environment.TickCount >= tick + 1000)
        {
            ups = count;
            count = 0;
            tick = Environment.TickCount;

            UpdateUps?.Invoke(ups);
        }
        else
        {
            count++;
        }
    }

    private void ReceiveSocketData()
    {
        for (var n = 1; n <= Connection.HighIndex; n++)
            if (Connection.Connections.ContainsKey(n))
            {
                Connection.Connections[n].ReceiveData();

                RemoveWhenNotConnected(n);
            }
    }

    private void RemoveWhenNotConnected(int index)
    {
        // Verifica se há dados disponíveis sem bloquear.

        if (!Connection.Connections[index].Connected)
        {
            Connection.Connections[index].Disconnect();
            Connection.Remove(index);
        }
    }
}