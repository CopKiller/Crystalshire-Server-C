using Database.Entities.ValueObjects.Player;
using GameServer.Communication;
using GameServer.Data.Configuration.Class;
using SharedLibrary.Util;

namespace GameServer.Data.Configuration
{
    public class Configuration
    {
        // Path Data
        private string PATH_DATA = "~/Data/".MyDir();

        // Archive Class Data
        private const string CLASS_DATA_FILE = "class.json";
        public static ClassData ClassData { get; set; }

        public void InitConfiguration()
        {
            // Validar diretório de dados
            VerificarECreateDiretorio(PATH_DATA);

            // Init Configuration
            LoadClassData();
        }

        #region Classes
        public void LoadClassData()
        {
            try
            {
                ClassData = ReaderWriterJson.Read<ClassData>(PATH_DATA + CLASS_DATA_FILE);

                if (ClassData == null)
                {
                    // Se a leitura do arquivo não foi bem-sucedida, podemos regenerar os dados e salvar
                    Global.WriteLog(LogType.System, "Class data not found or invalid. Regenerating...", ConsoleColor.Yellow);
                    SaveClassData();
                }
            }
            catch (Exception ex)
            {
                Global.WriteLog(LogType.System, $"Error loading class data: {ex.Message}", ConsoleColor.Red);
            }
        }
        public void SaveClassData()
        {
            ClassData = GenerateDefaultClassData();

            ReaderWriterJson.Write<ClassData>(PATH_DATA + CLASS_DATA_FILE, ClassData);
        }
        private ClassData GenerateDefaultClassData()
        {
            var Classe = new CharacterClass[3];

            // Dark Mage
            Classe[0] = new CharacterClass()
            {
                Name = "Dark Mage",
                Stats = new Stat()
                {
                    Strength = 10,
                    Endurance = 10,
                    Intelligence = 5,
                    Agility = 5,
                    WillPower = 5
                },
                MaleSprite = new int[] { 1, 2, 3, 4 },
                FemaleSprite = new int[] { 2, 1, 5, 7 },
                StartItem = new ItemData[] { new ItemData() { ItemId = 1, ItemCount = 1 } },
                StartSpellId = new int[] { 1 }
            };

            // WhatEver
            Classe[1] = new CharacterClass()
            {
                Name = "WhatEver",
                Stats = new Stat()
                {
                    Strength = 10,
                    Endurance = 10,
                    Intelligence = 5,
                    Agility = 5,
                    WillPower = 5
                },
                MaleSprite = new int[] { 3, 2, 3 },
                FemaleSprite = new int[] { 4, 3, 4 },
                StartItem = new ItemData[] { new ItemData() { ItemId = 1, ItemCount = 1 } },
                StartSpellId = new int[] { 1 }
            };

            // Warrior
            Classe[2] = new CharacterClass()
            {
                Name = "Warrior",
                Stats = new Stat()
                {
                    Strength = 10,
                    Endurance = 10,
                    Intelligence = 5,
                    Agility = 5,
                    WillPower = 5
                },
                MaleSprite = new int[] { 1, 4, 3 },
                FemaleSprite = new int[] { 2, 8, 9 },
                StartItem = new ItemData[] { new ItemData() { ItemId = 1, ItemCount = 1 } },
                StartSpellId = new int[] { 1 }
            };

            var classData = new ClassData
            {
                MaxClasses = 3,
                Classes = new Dictionary<int, CharacterClass>()
            };

            for (int i = 0; i < 3; i++)
            {
                classData.Classes.Add(i + 1, Classe[i]);
            }

            return classData;
        }
        #endregion

        static bool VerificarECreateDiretorio(string diretorio)
        {
            if (!Directory.Exists(diretorio))
            {
                try
                {
                    Directory.CreateDirectory(diretorio);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao criar o diretório: {ex.Message}");
                    return false;
                }
            }
            else
            {
                return true; // O diretório já existe
            }
        }
    }
}
