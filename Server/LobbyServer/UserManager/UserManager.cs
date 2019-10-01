using NHibernate;
using NHibernate.Criterion;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer
{
    public static class UserManager
    {
        public static void Add(Model.UserInfo user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(user);
                    transaction.Commit();
                }
            }
        }

        public static void Update(Model.UserInfo user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(user);
                    transaction.Commit();
                }
            }
        }

        public static void Remove(Model.UserInfo user)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(user);
                    transaction.Commit();
                }
            }
        }

        public static Model.UserInfo GetByID(int id)
        {
            Model.UserInfo userInfo = null;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    userInfo = session.Get<Model.UserInfo>(id);
                    transaction.Commit();
                }
            }
            return userInfo;
        }

        public static Model.UserInfo GetByUserName(string userName)
        {
            Model.UserInfo userInfo = null;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                ICriteria criteria = session.CreateCriteria(typeof(Model.UserInfo));
                criteria.Add(Restrictions.Eq("UserName", userName));
                userInfo = criteria.UniqueResult<Model.UserInfo>();
            }
            return userInfo;
        }

        public static ICollection<Model.UserInfo> GetAllUsers()
        {
            IList<Model.UserInfo> users = null;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                users = session.CreateCriteria(typeof(Model.UserInfo)).List<Model.UserInfo>();
            }
            return users;
        }
    }
}
