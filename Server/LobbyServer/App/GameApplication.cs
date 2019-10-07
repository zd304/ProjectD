using System;
using System.Collections.Generic;
using System.Text;
using Photon.SocketServer;
using NHibernate;
using NHibernate.Cfg;

namespace LobbyServer
{
    public class ClientInfo
    {
        public string userName;
        public int roomID;
        public GameClientPeer client;
    }

    public partial class GameApplication : ApplicationBase
    {
        List<GameClientPeer> clientPeers = new List<GameClientPeer>();
        Dictionary<GameClientPeer, ClientInfo> clientInfos = new Dictionary<GameClientPeer, ClientInfo>();

        Dictionary<int, Room> rooms = new Dictionary<int, Room>();
        static int roomGlobalID = 1;
        Dictionary<Operation.OperationCode, BaseHandler> handlers = new Dictionary<Operation.OperationCode, BaseHandler>();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            if (initRequest.LocalPort == 4520)
            {
                Debug.Log("连接到Game服务器：" + initRequest.LocalIP);
                GameServerPeer serverPeer = new GameServerPeer(initRequest);
                return serverPeer;
            }
            GameClientPeer clientPeer = new GameClientPeer(initRequest);
            clientPeers.Add(clientPeer);
            return clientPeer;
        }

        /// <summary>
        /// 启动服务器（当整个服务器启动起来的时候调用这个初始化）
        /// </summary>
        protected override void Setup()
        {
            System.Diagnostics.Debugger.Launch();

            RegisterHandlers();
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

        public void RemovePeer(GameClientPeer clientPeer)
        {
            clientPeers.Remove(clientPeer);
            ClientInfo clientInfo = null;
            if (clientInfos.TryGetValue(clientPeer, out clientInfo))
            {
                clientInfos.Remove(clientPeer);
            }
            if (clientInfo != null)
            {
                var room = GetRoom(clientInfo.roomID);
                if (room != null)
                {
                    room.RemoveClient(clientInfo);
                }
            }
        }

        public void AddClientInfo(GameClientPeer clientPeer, string userName)
        {
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.userName = userName;
            clientInfo.client = clientPeer;
            clientInfo.roomID = 0;
            clientInfos.Add(clientPeer, clientInfo);
        }

        public ClientInfo GetClientInfo(GameClientPeer peer)
        {
            ClientInfo clientInfo = null;
            if (!clientInfos.TryGetValue(peer, out clientInfo))
            {
                return null;
            }
            return clientInfo;
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

        public void RegisterHandler(BaseHandler handler)
        {
            handlers.Add(handler.OpCode, handler);
        }

        public void UnregisterHandler(Operation.OperationCode opCode)
        {
            handlers.Remove(opCode);
        }

        public void JoinRoom(GameClientPeer client)
        {
            foreach (var kv in rooms)
            {
                Room room = kv.Value;
                if (room.clients.Count < room.maxClient)
                {
                    ClientInfo clientInfo = null;
                    if (clientInfos.TryGetValue(client, out clientInfo))
                    {
                        room.AddClient(clientInfo);
                    }
                    return;
                }
            }
            Room r = CreateRoom(Room.NormalRoomMaxClient);
            ClientInfo ci = null;
            if (clientInfos.TryGetValue(client, out ci))
            {
                r.AddClient(ci);
            }
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

        private Room CreateRoom(int maxClient)
        {
            Room room = new Room();
            room.roomID = roomGlobalID++;
            room.maxClient = maxClient;
            rooms.Add(room.roomID, room);
            return room;
        }
    }
}
