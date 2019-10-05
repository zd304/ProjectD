using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Operation;

public class LoginRequest : Request
{
    public string username;
    public string password;

    public LoginRequest()
        : base(OperationCode.Login)
    {
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
        if (returnCode == Operation.ReturnCode.Success)
        {
            LoginSuccessResponse obj = PackageHelper.Desirialize<LoginSuccessResponse>(returnData);
            SceneManager.LoadScene("MonoPoly1");

            PhotonEngine.Instance.Reconnect(obj.ip, obj.port, obj.application);
        }
    }

    public override void DoRequest(byte[] data)
    {
        Dictionary<byte, object> customParameters = new Dictionary<byte, object>();
        customParameters[0] = data;
        PhotonEngine.Instance.Peer.OpCustom((byte)OpCode, customParameters, true);
    }

    public override void DoRequest<T>(T obj)
    {
        byte[] bytes = PackageHelper.Serialize<T>(obj);
        DoRequest(bytes);
    }
}