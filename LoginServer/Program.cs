using LoginServer.Communication;
using LoginServer.Database;
using LoginServer.Server;
using SharedLibrary.Util;

namespace LoginServer;

public static class Program
{
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>

    enum MenuDb
    {
        AdicionarConta = 1,
        AtualizarConta = 2,
        RecuperarConta = 3,
        ExcluirConta = 4
    }
    private static void Main()
    {
        var initServer = new InitServer();
        // Iniciar uma tarefa em segundo plano
        var backgroundTask = Task.Run(() => initServer.InitServerAsync());

        var dbStart = new DatabaseStartup(DatabaseStartup.Configure());

        var list = new List<String>();
        list.Add("1 - adicionar conta");
        list.Add("2 - atualizar contas");
        list.Add("3 - recuperar conta");
        list.Add("4 - excluir conta");

        while (true)
        {
            Global.WriteLog(LogType.System, "<<Menu>>", ConsoleColor.Yellow);
            foreach (var item in list)
            {
                Global.WriteLog(LogType.System, item, ConsoleColor.Yellow);
            }
            Global.WriteLog(LogType.System, String.Empty, ConsoleColor.Yellow);

            int.TryParse(Console.ReadLine(), out int response);

            if (response == (int)MenuDb.AdicionarConta)
            {
                Global.WriteLog(LogType.System, "[DATABASE] Digite o login:", ConsoleColor.Yellow);

                var _login = Console.ReadLine() ?? Console.ReadLine();

                Global.WriteLog(LogType.System, "[DATABASE] Digite a senha:", ConsoleColor.Yellow);

                var _senha = Console.ReadLine() ?? Console.ReadLine();

                Global.WriteLog(LogType.System, "[DATABASE] Digite o email:", ConsoleColor.Yellow);

                var _email = Console.ReadLine() ?? Console.ReadLine();

                _ = dbStart.AdicionarConta(_login, _senha, _email);
            }
            else if (response == (int)MenuDb.RecuperarConta)
            {
                Global.WriteLog(LogType.System, "[DATABASE] Digite o login:", ConsoleColor.Yellow);

                var _login = Console.ReadLine() ?? Console.ReadLine();

                _ = dbStart.RecuperarConta(_login);
            }
            else if (response == (int)MenuDb.AtualizarConta)
            {
                _ = dbStart.AtualizarContas();
            }
            else if (response == (int)MenuDb.ExcluirConta)
            {
                Global.WriteLog(LogType.System, "[DATABASE] Digite o login:", ConsoleColor.Yellow);

                var _login = Console.ReadLine() ?? Console.ReadLine();

                _ = dbStart.ExcluirConta(_login);
            }
            else if (response == (int)MenuDb.AtualizarConta)
            {
                _ = dbStart.AtualizarContas();
            }
            else
            {
                Global.WriteLog(LogType.System, "[DATABASE] Comando Invalido", ConsoleColor.Red);
            }
        }
    }
}