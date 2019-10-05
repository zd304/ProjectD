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

    [ProtoContract]
    public class LoginSuccessResponse
    {
        [ProtoMember(1)]
        public string ip;
        [ProtoMember(2)]
        public string port;
        [ProtoMember(3)]
        public string application;
    }
}