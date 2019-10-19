using Operation;

public class JoinRoomHandler : Handler
{
    public JoinRoomHandler() : base(OperationCode.JoinRoom) { }

    public override void OnEvent(byte[] data)
    {
        if (Game.Instance != null)
        {
            Game.Instance.selectRace.gameObject.SetActive(true);
        }
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
    }
}
