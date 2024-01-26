using LoginServer.Server;
using Microsoft.Extensions.Logging;
using SharedLibrary.Network;
using SharedLibrary.Util;

namespace LoginServer.Communication;

public static class Global
{
    public static int Environment;

    public static Action<ByteBuffer> SendGameServerPacket { get; set; }

    //public static DataServer Server;
    public static Log? PlayerLogs { get; set; }
    public static Log? SystemLogs { get; set; }
    public static Log? DebugLogs { get; set; }
    public static Log? DatabaseLogs { get; set; }

    public static void CloseLog()
    {
        PlayerLogs.CloseFile();
        SystemLogs.CloseFile();
        DebugLogs.CloseFile();
        DatabaseLogs.CloseFile();

        PlayerLogs = null;
        SystemLogs = null;
        DebugLogs = null;
        DatabaseLogs = null;
    }

    public static void WriteLog(LogType type, string text, ConsoleColor color)
    {
        switch (type)
        {
            case LogType.Player:
                PlayerLogs.Write(type, text, color);
                break;
            case LogType.System:
                SystemLogs.Write(type, text, color);
                break;
            case LogType.Debug:
                DebugLogs.Write(type, text, color);
                break;
            case LogType.Database:
                DatabaseLogs.Write(type, text, color);
                break;
        }
    }

    public static void InitLogs()
    {
        //System
        SystemLogs = new Log("System")
        {
            Index = 0
        };

        //Player
        PlayerLogs = new Log("Player")
        {
            Index = 1
        };

        //Debug
        DebugLogs = new Log("Debug")
        {
            Index = 2
        };

        //Database
        DatabaseLogs = new Log("Database")
        {
            Index = 3
        };

        //System
        var result = SystemLogs.OpenFile();
        if (result)
            SystemLogs.Enabled = true;
        else
            WriteLog(LogType.System, "An error ocurred when trying to open the file log.", ConsoleColor.Red);
        //Player
        result = PlayerLogs.OpenFile();

        if (result)
            PlayerLogs.Enabled = true;
        else
            WriteLog(LogType.System, "An error ocurred when trying to open the file log.", ConsoleColor.Red);
        //Debug
        result = DebugLogs.OpenFile();

        if (result)
            DebugLogs.Enabled = true;
        else
            WriteLog(LogType.System, "An error ocurred when trying to open the file log.", ConsoleColor.Red);
        //Database
        result = DatabaseLogs.OpenFile();

        if (result)
            DatabaseLogs.Enabled = true;
        else
            WriteLog(LogType.System, "An error ocurred when trying to open the file log.", ConsoleColor.Red);

        WriteLog(LogType.System, "Logs Initialized...", ConsoleColor.Green);
    }
}