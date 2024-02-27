using GameServer.Network.PacketList.ServerPacket;
using GameServer.Server.Data;
using GameServer.Server.Player.JoinGame;
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
            CheckEquippedItems.Check(index);
            new SUpdateItemTo(1).Send(searchPlayer.Connection);
            //SendClasses.Send(index); --> Não feito, já foi enviado as classes ao logar.

            // Criando a database dos items, que é o próximo item a ser enviado.

            // Enviar uma confirmação de que o jogador está no jogo, para abrir a tela do jogo no cliente.
            new SInGame().Send(searchPlayer.Connection);
        }
    }
}
