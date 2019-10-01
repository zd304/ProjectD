using Photon.SocketServer;
using Operation;

namespace LobbyServer
{
    public abstract class BaseHandler
    {
        public OperationCode OpCode { get; private set; }

        public BaseHandler(OperationCode opCode)
        {
            OpCode = opCode;
        }

        public abstract void OnOperateRequest(byte[] bytes, ClientPeer peer, SendParameters sendParameters);
    }
}
