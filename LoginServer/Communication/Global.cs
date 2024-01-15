using LoginServer.Server;
using SharedLibrary.Util;

namespace LoginServer.Communication
{
    public static class Global
    {
        public static int Environment;
        public static Log? PlayerLogs { get; set; }
        public static Log? SystemLogs { get; set; }
        public static Log? DebugLogs { get; set; }

        public static DataServer Server;

        public static void WriteLog(LogType type, string text,ConsoleColor color)
        {
            switch (type)
            {
                case LogType.Player:
                    PlayerLogs.Write(text, color);
                    break;
                case LogType.System:
                    SystemLogs.Write(text, color);
                    break;
                case LogType.Debug:
                    DebugLogs.Write(text, color);
                    break;
            }
        }

        public static Task InitLogs()
        {
            //System
            Global.SystemLogs = new Log("System")
            {
                Index = 0
            };

            //Player
            Global.PlayerLogs = new Log("Player")
            {
                Index = 1
            };

            //Debug
            Global.DebugLogs = new Log("Debug")
            {
                Index = 2
            };

            //System
            var result = Global.SystemLogs.OpenFile();
            if (result)
            {
                Global.SystemLogs.Enabled = true;
            }
            else
            {
                Global.WriteLog(LogType.System, "An error ocurred when trying to open the file log.", ConsoleColor.Red);
            }
            //Player
            result = Global.PlayerLogs.OpenFile();

            if (result)
            {
                Global.PlayerLogs.Enabled = true;
            }
            else
            {
                Global.WriteLog(LogType.System, "An error ocurred when trying to open the file log.", ConsoleColor.Red);
            }
            //Debug
            result = Global.DebugLogs.OpenFile();

            if (result)
            {
                Global.DebugLogs.Enabled = true;
            }
            else
            {
                Global.WriteLog(LogType.System, "An error ocurred when trying to open the file log.", ConsoleColor.Red);
            }

            Global.WriteLog(LogType.System, $"Logs Initialized...", ConsoleColor.Green);
            return Task.CompletedTask;
        }


    }
}
