using Operation;
using UnityEngine.SceneManagement;

public class HandShakeHandler : Handler
{
    public HandShakeHandler() : base(OperationCode.HandShake) { }

    public override void OnEvent(byte[] data)
    {
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
        EventSystem.Dispatch(EventID.HandShake);
    }
}
