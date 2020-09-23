using Operation;
using UnityEngine.SceneManagement;

public class JoinGameServerHandler : Handler
{
    public JoinGameServerHandler() : base(OperationCode.JoinGameServer) { }

    public override void OnEvent(byte[] data)
    {
        JoinGameServer jgs = PackageHelper.Desirialize<JoinGameServer>(data);
        PhotonEngine.Instance.Reconnect(jgs.ip, jgs.port, jgs.application);

        SceneManager.LoadScene("Level");
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
        
    }
}