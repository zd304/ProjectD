using System;
using System.Collections.Generic;
using System.Text;
using Photon.SocketServer;
using NHibernate;
using NHibernate.Cfg;
using System.Net;

namespace GameServer
{
    public class GameApplication : ApplicationBase
    {
        private GameServerPeer outboundPeer;

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new GameClientPeer(initRequest);
        }

        /// <summary>
        /// 启动服务器（当整个服务器启动起来的时候调用这个初始化）
        /// </summary>
        protected override void Setup()
        {
            System.Diagnostics.Debugger.Launch();

            outboundPeer = new GameServerPeer(this);
            outboundPeer.ConnectTcp(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4520), "LobbyServer");

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
    }
}
