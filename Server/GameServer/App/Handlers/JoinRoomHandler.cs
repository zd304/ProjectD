using Photon.SocketServer;
using Operation;

namespace GameServer
{
    public class JoinRoomHandler : BaseHandler
    {
        public JoinRoomHandler() : base(OperationCode.JoinRoom) { }

        public override void OnOperateRequest(byte[] bytes, ClientPeer peer, SendParameters sendParameters)
        {
            JoinRoom joinRoom = PackageHelper.Desirialize<JoinRoom>(bytes);

            GameApplication application = GameApplication.Instance;
            if (application == null)
            {
                return;
            }

            Room room = application.GetRoom(joinRoom.roomID);
            if (room == null)
            {
                return;
            }
            room.JoinRoom(peer as GameClientPeer, joinRoom.userName);
        }
    }
}
