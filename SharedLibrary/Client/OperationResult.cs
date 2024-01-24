
using System;
using System.Linq;
using System.Net.Http.Headers;

namespace SharedLibrary.Client
{
    public class OperationResult
    {
        //public static Dictionary<ClientMessages, OperationResult> Operations { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
        //Isso foi implementado pra converter pro tipo de dado que o cliente precisa
        public ClientMessages ClientMessages { get; set; }
        public ClientMenu ClientMenu { get; set; }
    }

    public class CombinedOperationResult<T> : OperationResult
    {
        public OperationResult BaseOperationResult { get; set; } = new OperationResult();
        public T Entity { get; set; } = default(T);
    }

}
