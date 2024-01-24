namespace SharedLibrary.Network.Interface;

public interface IConnection
{
    string UniqueKey { get; set; }
    string IpAddress { get; }
    bool Connected { get; }
    void Send(ByteBuffer msg, string name);
    void ReceiveData();
    void Disconnect();
}