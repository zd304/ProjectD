using System;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace GameServer
{
    public class GameClientPeer : ClientPeer
    {
        public GameClientPeer(InitRequest rqst)
            : base(rqst)
        {

        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
        }
    }
}
