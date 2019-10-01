using System;
using System.Collections.Generic;
using System.Text;
using Photon.SocketServer;
using NHibernate;
using NHibernate.Cfg;

namespace LobbyServer
{
    public partial class GameApplication : ApplicationBase
    {
        List<GameClientPeer> clientPeers = new List<GameClientPeer>();
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
    }
}
