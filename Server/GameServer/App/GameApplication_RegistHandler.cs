using Photon.SocketServer;

namespace GameServer
{
    public partial class GameApplication : ApplicationBase
    {
        private void RegisterHandlers()
        {
            RegisterHandler(new JoinRoomHandler());
        }

        private void RegisterS2SHandlers()
        {
            RegisterS2SHandler(new S2SCreateRoomHandler());
        }
    }
}
