using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Entities.Player;

namespace GameServer.Server.Player.JoinGame
{
    public class CheckEquippedItems
    {
        public static void Check(int index)
        {
            var searchPlayer = Authentication.Authentication.FindCharByIndex(index);

            var equipmentMaxIndex = Enum.GetValues(typeof(EquipmentType)).Length;
            // Verificar se o jogador está com algum item equipado.
            for (var equipmentNum = 0; equipmentNum < equipmentMaxIndex; equipmentNum++ )
            {
                var equipment = searchPlayer.Equipment[equipmentNum];
                if (equipment.ItemId > 0)
                {
                    // Falta obter o tipo do item que ele está equipado e verificar se é do tipo
                    // que pode ser equipado no slot em questão.
                    // A parte para verificar o tipo do item pelo número será implementada futuramente

                    // Caso o jogador não esteja equipamento o item no slot correto, remover o item.
                }
            }
        }
    }
}
