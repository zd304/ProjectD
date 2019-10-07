using System;
using System.Collections.Generic;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace LobbyServer
{
    public class GameClientPeer : ClientPeer
    {
        public GameClientPeer(InitRequest rqst)
            : base(rqst)
        {

        }

        public void SendEvent<T>(Operation.OperationCode opCode, T obj)
        {
            EventData data = new EventData();
            data.Code = (byte)opCode;

            byte[] bytes = PackageHelper.Serialize<T>(obj);

            Dictionary<byte, object> paramter = new Dictionary<byte, object>();
            paramter.Add(0, bytes);
            data.Parameters = paramter;
            SendEvent(data, new SendParameters());
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }
            application.RemovePeer(this);
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }
            BaseHandler handler = application.GetHandler((Operation.OperationCode)operationRequest.OperationCode);
            if (handler == null)
            {
                return;
            }
            byte[] bytes = (byte[])operationRequest.Parameters[0];
            handler.OnOperateRequest(bytes, this, sendParameters);
        }
    }
}
