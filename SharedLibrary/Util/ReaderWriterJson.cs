

using SharedLibrary.Client;
using System;
using System.Text.Json;

namespace SharedLibrary.Util
{
    public static class ReaderWriterJson
    {
        public static T Read<T>(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);

                    return JsonSerializer.Deserialize<T>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao ler o arquivo JSON: {ex.Message}");
                    return default; // Ou lançar uma exceção, dependendo do seu cenário
                }
            }
            else
            {
                Console.WriteLine($"O arquivo {path} não foi encontrado.");
                return default; // Ou lançar uma exceção, dependendo do seu cenário
            }
        }

        public static void Write<T>(string path, T data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao escrever no arquivo JSON: {ex.Message}");
                // Ou lançar uma exceção, dependendo do seu cenário
            }
        }

        public static async Task<T> ReadAsync<T>(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(path);

                    return JsonSerializer.Deserialize<T>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao ler o arquivo JSON: {ex.Message}");
                    return default; // Ou lançar uma exceção, dependendo do seu cenário
                }
            }
            else
            {
                Console.WriteLine($"O arquivo {path} não foi encontrado.");
                return default; // Ou lançar uma exceção, dependendo do seu cenário
            }
        }

        public static async Task WriteAsync<T>(string path, T data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(path, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao escrever no arquivo JSON: {ex.Message}");
                // Ou lançar uma exceção, dependendo do seu cenário
            }
        }
    }

}
