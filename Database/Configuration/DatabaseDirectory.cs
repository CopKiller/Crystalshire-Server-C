using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database.Configuration
{
    public static class DatabaseDirectory
    {
        public static string GetDatabaseDirectory()
        {
            // Obtém o diretório do arquivo de origem do assembly que está sendo executado
            string diretorioAssembly = Path.GetDirectoryName(Environment.CurrentDirectory);
            // Navega para o diretório pai, que é o diretório raiz do projeto em muitos casos
            string diretorioRaizProjeto = Path.GetFullPath(Path.Combine(diretorioAssembly, @"..\..\..\"));
            // Nome do arquivo de banco de dados
            diretorioRaizProjeto = "Data Source=" + diretorioRaizProjeto + "DatabaseSqlite.db";

            return diretorioRaizProjeto;
        }

        //public static string GetDatabaseDirectory()
        //{
        //    // Obtém o caminho do diretório do assembly em execução
        //    string diretorioAssembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        //    // Navega para cima até chegar ao diretório do banco de dados
        //    string caminhoBancoDeDados = Path.Combine(diretorioAssembly, "..", "..", "..", "DatabaseSqlite.db");

        //    // Obtém o caminho completo
        //    caminhoBancoDeDados = Path.GetFullPath(caminhoBancoDeDados);

        //    return caminhoBancoDeDados;
        //}
    }
}
