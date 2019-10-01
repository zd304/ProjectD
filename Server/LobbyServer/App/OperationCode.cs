using Photon.SocketServer;
using System;
using System.Collections.Generic;

namespace Operation
{
    public enum OperationCode : byte
    {
        Login,
        Regist,
    }
}

namespace LobbyServer
{
    public partial class GameApplication : ApplicationBase
    {
        private void RegisterHandlers()
        {
            RegisterHandler(new LoginHandler());
            RegisterHandler(new RegistHandler());
        }
    }
}