using Photon.SocketServer;
using System;
using System.Collections.Generic;

namespace Operation
{
    public enum OperationCode : byte
    {
        Login,
    }
}

namespace LobbyServer
{
    public partial class GameApplication : ApplicationBase
    {
        private void RegisterHandlers()
        {
            RegisterHandler(new LoginHandler());
        }
    }
}