


using GameServer.Data.Configuration;
using SharedLibrary.Network;

namespace GameServer.Network.ServerPacket;

public sealed class SClassesData : SendPacket
{
    public SClassesData()
    {
        msg = new ByteBuffer();
        msg.Write((int)OpCode.SendPacket[GetType()]);
        
        msg.Write(Configuration.ClassData.MaxClasses);

        for (var i = 0; i < Configuration.ClassData.MaxClasses; i++)
        {
            var vital = Configuration.ClassData.Classes[i].GetClassVitals();


            msg.Write(Configuration.ClassData.Classes[i].Name);
            msg.Write(vital.MaxHealth);
            msg.Write(vital.MaxEnergy);

            var length = Configuration.ClassData.Classes[i].MaleSprite.Length;
            msg.Write(length);

            for (var j = 0; j < length; j++)
            {
                msg.Write(Configuration.ClassData.Classes[i].MaleSprite[j]);
            }

            length = Configuration.ClassData.Classes[i].FemaleSprite.Length;

            msg.Write(length);

            for (var j = 0; j < length; j++)
            {
                msg.Write(Configuration.ClassData.Classes[i].FemaleSprite[j]);
            }

            var stat = Configuration.ClassData.Classes[i].GetClassStats();

            msg.Write(stat.Strength);
            msg.Write(stat.Endurance);
            msg.Write(stat.Intelligence);
            msg.Write(stat.Agility);
            msg.Write(stat.WillPower);
        }
    }
}