using Photon.SocketServer;
using Operation;

namespace LobbyServer
{
    public class LoginHandler : BaseHandler
    {
        public LoginHandler() : base(OperationCode.Login) { }

        public override void OnOperateRequest(byte[] bytes, ClientPeer peer, SendParameters sendParameters)
        {
            Operation.UserInfo userInfo = SerializeHelper.Desirialize<Operation.UserInfo>(bytes);
            Model.UserInfo dbUser = UserManager.GetByUserName(userInfo.username);

            OperationResponse response = new OperationResponse((byte)OperationCode.Login);
            if (dbUser.Password == userInfo.password)
            {
                response.ReturnCode = (short)ReturnCode.Success;
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Failed;
            }
            // response.Parameters[0] = ;
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
