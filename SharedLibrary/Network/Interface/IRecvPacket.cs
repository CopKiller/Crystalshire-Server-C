namespace SharedLibrary.Network.Interface
{
    public interface IRecvPacket
    {
        void Process(byte[] buffer, IConnection connection);
    }
}