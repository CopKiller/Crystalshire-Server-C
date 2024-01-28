using GameServer.Data.Configuration;
using SharedLibrary.Network;

namespace GameServer.Network.PacketList.ServerPacket;

public sealed class SClassesData : SendPacket
{
    public SClassesData()
    {
        //msg = new ByteBuffer();
        msg.Write((int)OpCode.SendPacket[GetType()]);

        msg.Write(Configuration.ClassData.MaxClasses);

        for (var i = 1; i <= Configuration.ClassData.MaxClasses; i++)
        {
            var vital = Configuration.ClassData.Classes[i].GetClassVitals();


            msg.Write(Configuration.ClassData.Classes[i].Name);
            msg.Write(vital.MaxHealth);
            msg.Write(vital.MaxEnergy);

            var length = Configuration.ClassData.Classes[i].MaleSprite.Length;
            msg.Write(length - 1);

            for (var j = 0; j < length; j++)
            {
                msg.Write(Configuration.ClassData.Classes[i].MaleSprite[j]);
            }

            length = Configuration.ClassData.Classes[i].FemaleSprite.Length;

            msg.Write(length - 1);

            for (var j = 0; j < length; j++)
            {
                msg.Write(Configuration.ClassData.Classes[i].FemaleSprite[j]);
            }

            var stat = Configuration.ClassData.Classes[i].GetClassStats();

            msg.Write(Convert.ToByte(stat.Strength));
            msg.Write(Convert.ToByte(stat.Endurance));
            msg.Write(Convert.ToByte(stat.Intelligence));
            msg.Write(Convert.ToByte(stat.Agility));
            msg.Write(Convert.ToByte(stat.WillPower));
        }
    }
}