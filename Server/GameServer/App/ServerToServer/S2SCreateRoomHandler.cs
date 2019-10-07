using Operation;
using Photon.SocketServer;

namespace GameServer
{
    public class S2SCreateRoomHandler : S2SBaseHandler
    {
        public S2SCreateRoomHandler() : base(S2SOperationCode.CreateRoom) { }

        public override void OnEvent(byte[] data)
        {
        }

        public override void OnOperateRequest(byte[] bytes, GameServerPeer peer, SendParameters sendParameters)
        {
            S2SCreateRoom roomData = PackageHelper.Desirialize<S2SCreateRoom>(bytes);
            GameApplication application = GameApplication.Instance as GameApplication;
            if (application == null)
            {
                return;
            }
            Room room = application.CreateRoom(roomData.roomID, roomData.members.Count);
            for (int i = 0; i < roomData.members.Count; ++i)
            {
                S2SRoomMember member = roomData.members[i];
                room.userNames.Add(member.userName);
            }

            OperationResponse response = new OperationResponse((byte)S2SOperationCode.CreateRoom);
            response.ReturnCode = (byte)ReturnCode.Success;
            PackageHelper.SetData(response, bytes);
            peer.SendOperationResponse(response, sendParameters);
        }

        public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
        {
        }
    }
}
