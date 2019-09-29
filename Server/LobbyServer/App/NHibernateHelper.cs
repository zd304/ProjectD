using NHibernate;
using NHibernate.Cfg;
using System;

namespace LobbyServer
{
    public class NHibernateHelper
    {
        private static ISessionFactory sessionFactory = null;

        public static void Initialize()
        {
            if (sessionFactory == null)
            {
                try
                {
                    Configuration cfg = new Configuration();
                    cfg.Configure(); // 解析hibernate.cfg.xml
                    cfg.AddAssembly("LobbyServer"); // 解析映射文件，UserInfo.hbm.xml...

                    sessionFactory = cfg.BuildSessionFactory();

                    Debug.Log("NHibernate初始化完成");
                }
                catch (Exception e)
                {
                    Debug.LogError("NHibernate初始化失败：" + e.Message);
                }
            }
        }

        public static void Uninitialize()
        {
            sessionFactory.Close();
            sessionFactory = null;
        }

        public static ISession OpenSession()
        {
            if (sessionFactory == null)
            {
                return null;
            }
            return sessionFactory.OpenSession();
        }

        public static void CloseSession(ISession session)
        {
            session.Close();
        }
    }
}
