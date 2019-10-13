using System;
using System.Collections.Generic;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace GameServer
{
    public class GameClientPeer : ClientPeer
    {
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
        }

        public Time Time
        {
            private set;
            get;
        }
    }
}
