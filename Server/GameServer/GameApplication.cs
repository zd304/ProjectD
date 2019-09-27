using System;
using System.Collections.Generic;
using System.Text;
using Photon.SocketServer;
using NHibernate;
using NHibernate.Cfg;

namespace GameServer
{
    class GameApplication : ApplicationBase
    {


        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new GamePeer(initRequest);
        }

        protected override void Setup()
        {
            System.Diagnostics.Debugger.Launch();

            Configuration cfg = new Configuration();
            cfg.Configure(); // 解析hibernate.cfg.xml
            cfg.AddAssembly("GameServer"); // 解析映射文件，UserInfo.hbm.xml...

            ISessionFactory sessionFactory = null;
            ISession session = null;
            try
            {
                sessionFactory = cfg.BuildSessionFactory();
                session = sessionFactory.OpenSession();
                GameServer.Model.UserInfo user = new Model.UserInfo() { UserName = "zd304", Password = "123456" };
                session.Save(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (session != null)
                {
                    session.Close();
                }
                if (sessionFactory != null)
                {
                    sessionFactory.Close();
                }
            }
        }

        protected override void TearDown()
        {
        }
    }
}
