using Photon.SocketServer;
using System;
using System.Collections.Generic;

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