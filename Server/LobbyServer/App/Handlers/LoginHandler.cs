﻿using Photon.SocketServer;
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
            obj.application = "GameServer";
            obj.ip = "127.0.0.1";
            obj.port = "5056";
            PackageHelper.SetData(response, PackageHelper.Serialize<LoginSuccessResponse>(obj));
            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
