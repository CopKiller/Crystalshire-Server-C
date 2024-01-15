
namespace SharedLibrary.Network.Interface
{
    public interface IConnection
    {
        void Send(ByteBuffer msg, string className);
    }
}