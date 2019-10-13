using Photon.SocketServer;
using Operation;

namespace GameServer
{
    public class JoinRoomHandler : BaseHandler
    {
        public JoinRoomHandler() : base(OperationCode.JoinRoom) { }

        public override void OnOperateRequest(byte[] bytes, ClientPeer peer, SendParameters sendParameters)
        {

        }
    }
}
