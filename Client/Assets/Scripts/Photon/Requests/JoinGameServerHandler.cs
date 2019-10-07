using Operation;
using UnityEngine.SceneManagement;

public class JoinGameServerHandler : Handler
{
    public JoinGameServerHandler() : base(OperationCode.JoinGameServer) { }

    public override void OnEvent(byte[] data)
    {
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
        JoinGameServer jgs = PackageHelper.Desirialize<JoinGameServer>(returnData);
        PhotonEngine.Instance.Reconnect(jgs.ip, jgs.port, jgs.application);

        SceneManager.LoadScene("MonoPoly1");
    }
}