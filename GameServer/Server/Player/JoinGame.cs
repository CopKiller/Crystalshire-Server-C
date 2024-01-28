using GameServer.Network.PacketList.ServerPacket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Server.Player
{
    public class InitJoinGame
    {
        public static void JoinGame(int index, int charSlot)
        {
            var searchPlayer = Authentication.Authentication.Players[index];

            searchPlayer.CharSlot = charSlot;
            searchPlayer.GameState = GameState.Game;

            new SLoginOk(index).Send(searchPlayer.Connection);

            // Enviar todos os dados que precisam ser carregado no cliente.


            // Enviar uma confirmação de que o jogador está no jogo, para abrir a tela do jogo no cliente.
            new SInGame().Send(searchPlayer.Connection);
        }
    }
}
