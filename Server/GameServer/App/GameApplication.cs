using System;
using System.Collections.Generic;
using Photon.SocketServer;
using System.Net;

namespace GameServer
{
    public partial class GameApplication : ApplicationBase
    {
        private GameServerPeer outboundPeer;
        private List<GameClientPeer> clientPeerList = new List<GameClientPeer>();
        private Dictionary<Operation.OperationCode, BaseHandler> handlers = new Dictionary<Operation.OperationCode, BaseHandler>();
        private Dictionary<Operation.S2SOperationCode, S2SBaseHandler> s2sHandlers = new Dictionary<Operation.S2SOperationCode, S2SBaseHandler>();

        private Dictionary<int, Room> rooms = new Dictionary<int, Room>();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            GameClientPeer peer = new GameClientPeer(initRequest);
            clientPeerList.Add(peer);
            return peer;
        }

        /// <summary>
        /// 启动服务器（当整个服务器启动起来的时候调用这个初始化）
        /// </summary>
        protected override void Setup()
        {
            System.Diagnostics.Debugger.Launch();

            outboundPeer = new GameServerPeer(this);
            outboundPeer.ConnectTcp(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4520), "LobbyServer");

            RegisterHandlers();
            RegisterS2SHandlers();
            Debug.Initialize(this);
            NHibernateHelper.Initialize();

        }

        /// <summary>
        /// 关闭服务器时调用
        /// </summary>
        protected override void TearDown()
        {
            NHibernateHelper.Uninitialize();
            Debug.Uninitialize();
        }

        private void RegisterHandler(BaseHandler handler)
        {
            handlers.Add(handler.OpCode, handler);
        }

        public void UnregisterHandler(Operation.OperationCode opCode)
        {
            handlers.Remove(opCode);
        }

        public BaseHandler GetHandler(Operation.OperationCode opCode)
        {
            BaseHandler handler;
            if (!handlers.TryGetValue(opCode, out handler))
            {
                return null;
            }
            return handler;
        }

        private void RegisterS2SHandler(S2SBaseHandler handler)
        {
            s2sHandlers.Add(handler.OpCode, handler);
        }

        public void UnregisterS2SHandler(Operation.S2SOperationCode opCode)
        {
            s2sHandlers.Remove(opCode);
        }

        public S2SBaseHandler GetS2SHandler(Operation.S2SOperationCode opCode)
        {
            S2SBaseHandler handler;
            if (!s2sHandlers.TryGetValue(opCode, out handler))
            {
                return null;
            }
            return handler;
        }

        public Room GetRoom(int roomID)
        {
            Room room = null;
            if (!rooms.TryGetValue(roomID, out room))
            {
                return null;
            }
            return room;
        }

        public void RemoveRoom(Room room)
        {
            rooms.Remove(room.roomID);
        }

        public Room CreateRoom(int roomID, int maxClient)
        {
            Room room = new Room();
            room.roomID = roomID;
            room.maxClient = maxClient;
            rooms.Add(room.roomID, room);
            return room;
        }
    }
}
