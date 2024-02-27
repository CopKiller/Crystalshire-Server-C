using Database.Entities.Player;
using Database.Entities.ValueObjects.Player;
using Database.Entities.ValueObjects.Player.Interface;
using Microsoft.VisualBasic;
using SharedLibrary.Network;
using SharedLibrary.Network.Interface;
using SharedLibrary.Util;

namespace GameServer.Server.Data.Items
{
    public class ItemData
    {
        public const string ItemPath = @"Items\";
        public const string ItemExt = @".json";

        public const int MaxItems = 500;

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Sound { get; set; } = "";
        public int Pic { get; set; } = 0;
        public ItemType Type { get; set; } = ItemType.None;
        public int Data1 { get; set; } = 0;
        public int Data2 { get; set; } = 0;
        public int Data3 { get; set; } = 0;
        public ClassType ClassReq { get; set; } = 0;
        public AccessType AccessReq { get; set; } = 0;
        public int LevelReq { get; set; } = 0;
        public byte Mastery { get; set; } = 0;
        public int Price { get; set; } = 0;
        public Stat AddStat { get; set; } = new Stat();
        public byte Rarity { get; set; } = 0;
        public int Speed { get; set; } = 0;
        public int Handed { get; set; } = 0;
        public BindType BindType { get; set; } = 0;
        public Stat StatReq { get; set; } = new Stat();
        public int Animation { get; set; } = 0;
        public int Paperdoll { get; set; } = 0;
        // consume
        public ConsumeData Consume { get; set; } = new ConsumeData();
        // food
        public FoodData Food { get; set; } = new FoodData();
        // requirements
        public int Proficiency { get; set; } = 0;

        public static void InitItems(string Path_Data)
        {
            var combinePath = Path_Data + ItemPath;
            // Verificar se o diretório existe, se não, criar.
            if (!Directory.Exists(combinePath))
            {
                Directory.CreateDirectory(combinePath);
                CreateDefaultItems(Path_Data);
            }

            LoadItems(combinePath);
        }

        public static void CreateDefaultItems(string combinePath)
        {
            for (var i = 1; i <= MaxItems; i++)
            {
                var itemData = new ItemData();
                Configuration.Items.Add(itemData);
            }

        }

        public static void SaveItem(string combinePath, int ItemId)
        {
            ReaderWriterJson.Write(combinePath + ItemId + ItemExt, Configuration.Items[ItemId - 1]);
        }

        public static void LoadItems(string combinePath)
        {

            for (int i = 1; i <= MaxItems; i++)
            {
                var item = ReaderWriterJson.Read<ItemData>(combinePath + i + ItemExt);

                if (item == null) 
                {
                    var itemDefault = new ItemData();
                    Configuration.Items.Add(itemDefault);
                    SaveItem(combinePath, i);
                }
                else
                {
                    Configuration.Items.Add(item);
                }
            }
        }

        public static void SaveItems(string combinePath)
        {
            for (int i = 1; i <= MaxItems; i++)
            {
                if (Configuration.Items[i - 1] == null)
                {
                    SaveItem(combinePath, i);
                }
            }
        }

        public static void SendItems()
        {

        }
    }
}
