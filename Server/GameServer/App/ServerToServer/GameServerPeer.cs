using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;
using Operation;

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
            Debug.Log("连接Lobby服务器成功");
        }

        protected override void OnConnectionFailed(int errorCode, string errorMessage)
        {
            Debug.Log("连接Lobby服务器失败");
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {

        }

        protected override void OnEvent(IEventData eventData, SendParameters sendParameters)
        {
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }

            S2SBaseHandler handler = application.GetS2SHandler((S2SOperationCode)eventData.Code);
            if (handler == null)
            {
                return;
            }
            byte[] data = null;
            object p = null;
            if (eventData.Parameters.TryGetValue(0, out p))
            {
                data = p as byte[];
            }
            handler.OnEvent(data);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }

            S2SBaseHandler handler = application.GetS2SHandler((S2SOperationCode)operationRequest.OperationCode);
            if (handler == null)
            {
                return;
            }
            byte[] data = null;
            object p = null;
            if (operationRequest.Parameters.TryGetValue(0, out p))
            {
                data = p as byte[];
            }
            handler.OnOperateRequest(data, this, sendParameters);
        }

        protected override void OnOperationResponse(OperationResponse operationResponse, SendParameters sendParameters)
        {
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }

            S2SBaseHandler handler = application.GetS2SHandler((S2SOperationCode)operationResponse.OperationCode);
            if (handler == null)
            {
                return;
            }
            byte[] data = null;
            object p = null;
            if (operationResponse.Parameters.TryGetValue(0, out p))
            {
                data = p as byte[];
            }
            handler.OnOperationResponse((ReturnCode)operationResponse.ReturnCode, data);
        }
    }
}
