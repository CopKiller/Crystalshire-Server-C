namespace SharedLibrary.Util;

public sealed class Log
{
    private readonly string file = string.Empty;
    private FileStream stream;
    private StreamWriter writer;

    public Log(string name)
    {
        file = $"{name} {DateTime.Today.Year} - {DateTime.Today.Month} - {DateTime.Today.Day}.txt";
    }

    public bool Enabled { get; set; }
    public int Index { get; set; }

    public bool OpenFile()
    {
        try
        {
            if (!Directory.Exists("./Logs")) Directory.CreateDirectory("./Logs");

            stream = new FileStream($"./Logs/{file}", FileMode.Append, FileAccess.Write);
            writer = new StreamWriter(stream);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public void CloseFile()
    {
        if (stream != null)
        {
            writer.Close();
            stream.Close();

            writer.Dispose();
            stream.Dispose();
        }
    }

    private void Write(string text)
    {
        if (Enabled)
        {
            writer.WriteLine($"{text}");
            writer.Flush();
        }
    }

    public void Write(LogType logType,string text, ConsoleColor color)
    {
        text = $"[{DateTime.Now.ToString("dd/MM/yy - HH:mm")}] [{Enum.GetName(typeof(LogType), logType)}] {text}";

        Console.ForegroundColor = color;
        Console.WriteLine($"{text}");
        Console.ResetColor();

        Write(text);
    }
}