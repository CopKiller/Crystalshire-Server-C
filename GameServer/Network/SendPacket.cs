﻿using GameServer.Network.Interface;
using GameServer.Network.Tcp;
using GameServer.Server.Authentication;
using SharedLibrary.Network;

namespace GameServer.Network;

public abstract class SendPacket
{
    protected ByteBuffer msg;

    public SendPacket()
    {
        msg = new ByteBuffer();
    }

    public void Send(IConnection connection)
    {
        ((Connection)connection).Send(msg, GetType().Name);

        if (msg != null)
        {
            msg.Clear();
            msg = null;
        }
    }

    public void SendToAll()
    {
        var players = Authentication.Players;
        var highIndex = Authentication.HighIndex;

        for (var i = 1; i <= highIndex; i++)
        {
            if (players.ContainsKey(i))
            {
                if (players[i].Connected)
                {
                    Send(players[i].Connection);
                }
            }
        }
    }

    public void SendToAllBut(int index)
    {
        var players = Authentication.Players;
        var highIndex = Authentication.HighIndex;

        for (var i = 1; i <= highIndex; i++)
        {
            if (players.ContainsKey(i))
            {
                if (i != index)
                {
                    Send(players[i].Connection);
                }
            }
        }
    }

    public void SendToMap(int mapId)
    {
        var players = Authentication.Players;
        var highIndex = Authentication.HighIndex;

        for (var i = 1; i <= highIndex; i++)
        {
            if (players.ContainsKey(i))
            {
                var charIndex = players[i].Players.FindIndex(p => p.SlotId == players[i].CharSlot);

                if (charIndex >= 0)
                {
                    if (players[i].Players[charIndex].Position.MapNum == mapId)
                    {
                        Send(players[i].Connection);
                    }
                }
            }
        }
    }

    public void SendToMapBut(int index, int mapId)
    {
        var players = Authentication.Players;
        var highIndex = Authentication.HighIndex;

        for (var i = 1; i <= highIndex; i++)
        {
            if (i != index)
            {
                if (players.ContainsKey(i))
                {
                    var characterData = Authentication.FindCharByIndex(i);

                    if (characterData != null)
                    {
                        if (characterData.Position.MapNum == mapId)
                        {
                            Send(players[i].Connection);
                        }
                    }
                }
            }
        }
    }
}