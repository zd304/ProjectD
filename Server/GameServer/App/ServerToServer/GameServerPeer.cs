using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;

namespace GameServer
{
    public class GameServerPeer : OutboundS2SPeer
    {
        public GameServerPeer(ApplicationBase application)
            : base(application)
        {

        }

        protected override void OnConnectionEstablished(object responseObject)
        {
            Debug.Log("连接服务器成功");
        }

        protected override void OnConnectionFailed(int errorCode, string errorMessage)
        {
            Debug.Log("连接服务器失败");
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
        }
    }
}
