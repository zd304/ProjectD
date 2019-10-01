using Photon.SocketServer;
using Operation;

namespace LobbyServer
{
    class RegistHandler : BaseHandler
    {
        public RegistHandler() : base(OperationCode.Regist) { }

        public override void OnOperateRequest(byte[] bytes, ClientPeer peer, SendParameters sendParameters)
        {
            Operation.UserInfo userInfo = SerializeHelper.Desirialize<Operation.UserInfo>(bytes);
            Model.UserInfo dbUser = UserManager.GetByUserName(userInfo.username);

            OperationResponse response = new OperationResponse((byte)OperationCode.Regist);
            if (dbUser != null)
            {
                response.ReturnCode = (short)ReturnCode.Failed;
            }
            else
            {
                dbUser = new Model.UserInfo() { UserName = userInfo.username, Password = userInfo.password, RegisterDate = System.DateTime.Now };
                UserManager.Add(dbUser);
                response.ReturnCode = (short)ReturnCode.Success;
            }
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
