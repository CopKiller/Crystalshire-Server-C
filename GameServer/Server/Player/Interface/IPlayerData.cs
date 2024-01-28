using Database.Entities.Player;
using Database.Entities.ValueObjects.Player;
using GameServer.Network.Interface;

namespace GameServer.Server.PlayerData.Interface
{
    public interface IPlayerData
    {
        bool Connected { get; }
        int ConnectedTime { get; set; }
        IConnection Connection { get; set; }
        GameState GameState { get; set; }
        int Index { get; set; }
        string UniqueKey { get; set; }
        int CharSlot { get; set; }
        int AccountEntityId { get; }
        string Login { get; set; }
        List<PlayerEntity> Players { get; set; }
        void Save();
    }
}