using System;
using System.Collections.Generic;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using Operation;

namespace GameServer
{
    public class GameClientPeer : ClientPeer
    {
        bool handshake = false;

        public GameClientPeer(InitRequest rqst)
            : base(rqst)
        {
            Time = new Time();
            Time.Initialize();
            this.RequestFiber.ScheduleOnInterval(Update, 0L, 66L);
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Time.Uninitialize();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            GameApplication application = GameApplication.Instance;
            if (application == null)
            {
                return;
            }
            BaseHandler handler = application.GetHandler((OperationCode)operationRequest.OperationCode);
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

        private void Update()
        {
            if (Time != null)
            {
                Time.Update();
            }
            if (!handshake)
            {
                OperationResponse response = new OperationResponse((byte)OperationCode.HandShake);
                NullMessage nullMsg = new NullMessage();
                PackageHelper.SetData(response, PackageHelper.Serialize<NullMessage>(nullMsg));
                this.SendOperationResponse(response, new SendParameters());

                handshake = true;
            }
        }

        public Time Time
        {
            private set;
            get;
        }
    }
}
