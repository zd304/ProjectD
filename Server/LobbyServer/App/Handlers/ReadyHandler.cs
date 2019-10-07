using Photon.SocketServer;
using Operation;

namespace LobbyServer
{
    class ReadyHandler : BaseHandler
    {
        public ReadyHandler() : base(OperationCode.Ready) { }

        public override void OnOperateRequest(byte[] bytes, ClientPeer peer, SendParameters sendParameters)
        {
            ReadyData readyData = PackageHelper.Desirialize<ReadyData>(bytes);
            
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }

            Room room = application.GetRoom(readyData.roomID);
            ClientInfo clientInfo = application.GetClientInfo(peer as GameClientPeer);
            if (room != null && clientInfo != null)
            {
                room.Ready(clientInfo);
            }
        }
    }
}
