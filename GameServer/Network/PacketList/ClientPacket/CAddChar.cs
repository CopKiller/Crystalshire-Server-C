using Database.Entities.Player;
using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Network.PacketList.ServerPacket;
using GameServer.Server;
using GameServer.Server.Authentication;
using SharedLibrary.Client;
using SharedLibrary.Network;
using SharedLibrary.Util;
using System.Numerics;
using System.Text.RegularExpressions;

namespace GameServer.Network.PacketList.ClientPacket
{
    public sealed class CAddChar : IRecvPacket
    {
        private IConnection _connection;
        public void Process(byte[] buffer, IConnection connection)
        {
            var msg = new ByteBuffer(buffer);
            string name = msg.ReadString();
            int genderType = msg.ReadInt32();
            int classIndex = msg.ReadInt32();
            int spriteIndex = msg.ReadInt32();
            int characterIndex = msg.ReadInt32();

            var newPlayer = new PlayerEntity();
            newPlayer.Name = name;
            newPlayer.Sexo = (SexType)genderType;
            newPlayer.ClassType = (ClassType)classIndex;
            newPlayer.Sprite = spriteIndex;
            newPlayer.Id = characterIndex;

            if (newPlayer.Name == string.Empty)
            {
                Global.WriteLog(LogType.Player, "Player name is empty!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.NameIllegal, ClientMenu.MenuChars).Send(connection);
                return;
            }
            if (newPlayer.Name.Length > PlayerEntity.MaxNameCaracteres || newPlayer.Name.Length < 3)
            {
                Global.WriteLog(LogType.Player, $"Player name Length > {PlayerEntity.MaxNameCaracteres}!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.NameLength, ClientMenu.MenuChars).Send(connection);
                return;
            }
            if (newPlayer.Sexo > (SexType)Enum.GetValues(typeof(SexType)).Length || newPlayer.Sexo < 0)
            {
                Global.WriteLog(LogType.Player, $"Player Sex Invalid {newPlayer.Sexo}!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.Connection, ClientMenu.MenuChars).Send(connection);
                return;
            }
            if (newPlayer.ClassType > (ClassType)Enum.GetValues(typeof(ClassType)).Length || newPlayer.ClassType < 0)
            {
                Global.WriteLog(LogType.Player, $"Player Class Invalid {newPlayer.ClassType}!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.Connection, ClientMenu.MenuChars).Send(connection);
                return;
            }
            if (newPlayer.Sprite > 1000 || newPlayer.Sprite < 1)
            {
                Global.WriteLog(LogType.Player, $"Player Sprite Invalid {newPlayer.Sprite}!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.Connection, ClientMenu.MenuChars).Send(connection);
                return;
            }
            if (newPlayer.Id < 0)
            {
                Global.WriteLog(LogType.Player, $"Player Id Invalid {newPlayer.Id}!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.Connection, ClientMenu.MenuMain).Send(connection);
                return;
            }
            if (!Regex.IsMatch(newPlayer.Name, "^[a-zA-Z0-9_]+$"))
            {
                Global.WriteLog(LogType.Player, $"Player name invalid caracteres {newPlayer.Name}!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.NameIllegal, ClientMenu.MenuChars).Send(connection);
                return;
            }

            var Pers = new Character(newPlayer);

            var player = Authentication.FindByIndex(connection.Index);
            _connection = player.Connection;

            var players = Pers.Create(player.AccountEntityId);

            if (players.Result == null)
            {
                Global.WriteLog(LogType.Player, $"Erro ao criar o personagem {newPlayer.Name}!", ConsoleColor.Red);
                var alert = new SAlertMsg(ClientMessages.NameTaken, ClientMenu.MenuChars);
                alert.Send(_connection);
            }
            else
            {
                Global.WriteLog(LogType.Player, $"Personagem {newPlayer.Name} criado com sucesso!", ConsoleColor.Green);
                SendChars(players.Result);
            }
        }

        public void SendChars(List<PlayerEntity> chars)
        {
            var msgPlayerChars = new SPlayerChars(chars);
            msgPlayerChars.Send(_connection);
        }
    }
}
