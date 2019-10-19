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
        HandShake,
        JoinRoom,
    }

    public enum S2SOperationCode : byte
    {
        CreateRoom,
    }
}