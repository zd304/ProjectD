using Photon.SocketServer;
using Operation;
using System.Collections.Generic;

namespace LobbyServer
{
    public class MatchRequestHandler : BaseHandler
    {
        public MatchRequestHandler() : base(OperationCode.MatchRequest) { }

        public override void OnOperateRequest(byte[] bytes, ClientPeer peer, SendParameters sendParameters)
        {
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }

            application.JoinRoom(peer as GameClientPeer);
        }
    }
}
