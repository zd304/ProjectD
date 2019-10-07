using ProtoBuf;
using System.Collections.Generic;

namespace Operation
{
    [ProtoContract]
    public class NullMessage
    {

    }

    [ProtoContract]
    public class UserInfo
    {
        [ProtoMember(1)]
        public string username;
        [ProtoMember(2)]
        public string password;
    }

    [ProtoContract]
    public class RoomClient
    {
        [ProtoMember(1)]
        public string userName;
        [ProtoMember(2)]
        public bool ready;
    }

    [ProtoContract]
    public class RoomData
    {
        [ProtoMember(1)]
        public int roomID;
        [ProtoMember(2)]
        public List<RoomClient> clientInfos = new List<RoomClient>();
    }

    [ProtoContract]
    public class ReadyData
    {
        [ProtoMember(1)]
        public int roomID;
    }

    [ProtoContract]
    public class JoinGameServer
    {
        [ProtoMember(1)]
        public string ip;
        [ProtoMember(2)]
        public string port;
        [ProtoMember(3)]
        public string application;
    }

    [ProtoContract]
    public class LoginSuccessResponse
    {
        [ProtoMember(1)]
        public string userName;
    }
}