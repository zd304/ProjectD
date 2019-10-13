using Operation;

public class JoinRoomHandler : Handler
{
    public JoinRoomHandler() : base(OperationCode.JoinRoom) { }

    public override void OnEvent(byte[] data)
    {

    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
    }
}
