﻿using Photon.SocketServer;
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
            RegisterHandler(new ReadyHandler());
            RegisterHandler(new MatchRequestHandler());
        }

        private void RegisterS2SHandlers()
        {
            RegisterS2SHandler(new S2SCreateRoomHandler());
        }
    }
}