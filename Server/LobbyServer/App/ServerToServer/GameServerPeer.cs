using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;
using PhotonHostRuntimeInterfaces;
using System;
using System.Collections.Generic;
using Operation;

namespace LobbyServer
{
    public class GameServerPeer : InboundS2SPeer
    {
        public GameServerPeer(InitRequest initRequest)
            : base(initRequest)
        {

        }

        public void SendEvent<T>(Operation.S2SOperationCode opCode, T obj)
        {
            EventData data = new EventData();
            data.Code = (byte)opCode;

            byte[] bytes = PackageHelper.Serialize<T>(obj);

            Dictionary<byte, object> paramter = new Dictionary<byte, object>();
            paramter.Add(0, bytes);
            data.Parameters = paramter;
            SendEvent(data, new SendParameters());
        }

        public void DoRequest<T>(Operation.S2SOperationCode opCode, T obj)
        {
            byte[] data = PackageHelper.Serialize<T>(obj);

            Dictionary<byte, object> customParameters = new Dictionary<byte, object>();
            customParameters[0] = data;

            SendOperationRequest(new OperationRequest { OperationCode = (byte)opCode, Parameters = customParameters }, new SendParameters());
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
            object p;
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
            object p;
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
            object p;
            if (operationResponse.Parameters.TryGetValue(0, out p))
            {
                data = p as byte[];
            }
            handler.OnOperationResponse((ReturnCode)operationResponse.ReturnCode, data);
        }
    }
}
