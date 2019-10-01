using ProtoBuf;

namespace Operation
{
    [ProtoContract]
    public class UserInfo
    {
        [ProtoMember(1)]
        public string username;
        [ProtoMember(2)]
        public string password;
    }
}