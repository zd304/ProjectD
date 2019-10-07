namespace Operation
{
    public enum OperationCode : byte
    {
        // LobbyServer
        Login = 0,
        Regist,
        MatchRequest,
        RoomSync,
        Ready,
        JoinGameServer,

        // GameServer
        JoinRoom,
    }

    public enum S2SOperationCode : byte
    {
        CreateRoom,
    }
}