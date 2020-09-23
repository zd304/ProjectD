using Photon.SocketServer;
using Operation;
using System.Collections.Generic;

namespace LobbyServer
{
    public class LoginHandler : BaseHandler
    {
        public LoginHandler() : base(OperationCode.Login) { }

        public override void OnOperateRequest(byte[] bytes, ClientPeer peer, SendParameters sendParameters)
        {
            Operation.UserInfo userInfo = PackageHelper.Desirialize<Operation.UserInfo>(bytes);
            Model.UserInfo dbUser = UserManager.GetByUserName(userInfo.username);

            if (dbUser == null)
            {
                return;
            }

            OperationResponse response = new OperationResponse((byte)OperationCode.Login);
            if (dbUser.Password == userInfo.password)
            {
                response.ReturnCode = (short)ReturnCode.Success;
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Failed;
            }
            LoginSuccessResponse obj = new LoginSuccessResponse();
            obj.userName = userInfo.username;
            PackageHelper.SetData(response, PackageHelper.Serialize<LoginSuccessResponse>(obj));
            peer.SendOperationResponse(response, sendParameters);

            GameApplication application = GameApplication.Instance as GameApplication;
            if (application != null)
            {
                application.AddClientInfo(peer as GameClientPeer, userInfo.username);
            }
        }
    }
}
