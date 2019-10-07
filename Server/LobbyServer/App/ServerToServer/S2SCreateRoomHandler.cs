using Operation;
using Photon.SocketServer;

namespace LobbyServer
{
    public class S2SCreateRoomHandler : S2SBaseHandler
    {
        public S2SCreateRoomHandler() : base(S2SOperationCode.CreateRoom) { }

        public override void OnEvent(byte[] data)
        {
        }

        public override void OnOperateRequest(byte[] bytes, GameServerPeer peer, SendParameters sendParameters)
        {
        }

        public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
        {
            if (returnCode != ReturnCode.Success)
            {
                return;
            }
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }

            S2SCreateRoom roomData = PackageHelper.Desirialize<S2SCreateRoom>(returnData);
            for (int i = 0; i < roomData.members.Count; ++i)
            {
                S2SRoomMember member = roomData.members[i];
                ClientInfo clientInfo = application.GetClientInfo(member.userName);
                if (clientInfo == null)
                {
                    continue;
                }
                Operation.JoinGameServer jgs = new Operation.JoinGameServer();
                jgs.application = "GameServer";
                jgs.ip = "127.0.0.1";
                jgs.port = "5056";
                clientInfo.client.SendEvent<Operation.JoinGameServer>(Operation.OperationCode.JoinGameServer, jgs);
            }
        }
    }
}
