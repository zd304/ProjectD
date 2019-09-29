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
