using Operation;
using Photon.SocketServer;

namespace LobbyServer
{
    public abstract class S2SBaseHandler
    {
        public S2SOperationCode OpCode { get; private set; }

        public S2SBaseHandler(S2SOperationCode opCode)
        {
            OpCode = opCode;
        }

        public abstract void OnOperateRequest(byte[] bytes, GameServerPeer peer, SendParameters sendParameters);
        public abstract void OnOperationResponse(ReturnCode returnCode, byte[] returnData);
        public abstract void OnEvent(byte[] data);
    }
}
