using Database.Entities.Account;
using Database.Entities.Player;
using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Network.PacketList.ServerPacket;
using GameServer.Server;
using GameServer.Server.Authentication;
using GameServer.Server.Player;
using SharedLibrary.Client;
using SharedLibrary.Network;
using SharedLibrary.Util;
using System.Numerics;
using System.Text.RegularExpressions;

namespace GameServer.Network.PacketList.ClientPacket
{
    public sealed class CUseChar : IRecvPacket
    {
        public void Process(byte[] buffer, IConnection connection)
        {
            var msg = new ByteBuffer(buffer);
            int characterIndex = msg.ReadInt32();

            var player = Authentication.FindByIndex(connection.Index);

            // Se o jogador não estiver na tela de seleção de personagens, retorna para o menu principal
            if (player.GameState != GameState.Characters)
            {
                Global.WriteLog(LogType.Player, $"Player {player.Login} is not in the character selection!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.Connection, ClientMenu.MenuMain).Send(connection);
                return;
            }

            // Se o jogador estiver com uma versão desatualizada, retorna para o menu principal
            if (characterIndex < 1 || characterIndex > 3)
            {
                Global.WriteLog(LogType.Player, $"Player {player.Login} tried to use an invalid character index!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.Outdated, ClientMenu.MenuMain).Send(connection);
                return;
            }

            // Se o jogador não tiver um personagem criado, retorna para o menu principal

            var findCharacterSlot = player.Players.FindIndex(a => a.SlotId == characterIndex);
            if (findCharacterSlot == -1)
            {
                Global.WriteLog(LogType.Player, $"Player {player.Login} tried to use a character that does not exist!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.Connection, ClientMenu.MenuMain).Send(connection);
                return;
            }

            // O char existe, então vamos conectá-lo ao jogo.
            InitJoinGame.JoinGame(player.Index, characterIndex);
        }
    }
}
