using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer.Model
{
    public class UserInfo
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual DateTime RegisterDate { get; set; }
    }
}
