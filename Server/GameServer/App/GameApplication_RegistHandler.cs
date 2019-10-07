using Photon.SocketServer;

namespace GameServer
{
    public partial class GameApplication : ApplicationBase
    {
        private void RegisterHandlers()
        {

        }

        private void RegisterS2SHandlers()
        {
            RegisterS2SHandler(new S2SCreateRoomHandler());
        }
    }
}
