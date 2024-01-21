using Database.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories.ValidateData
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
        //Isso foi implementado pra converter pro tipo de dado que o cliente precisa
        public ClientMessages ClientMSG { get; set; }
        public ClientMenu ClientMenu { get; set; }
    }

    public class ClientOperationDictionary
    {
        public static Dictionary<ClientMessages, OperationResult> Result { get; set;}

        public static OperationResult GetResult(ClientMessages clientMSG)
        {
            return Result[clientMSG];
        }
    }
}
