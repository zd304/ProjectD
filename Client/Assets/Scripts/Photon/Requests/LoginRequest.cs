using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginRequest : Request
{
    public string username;
    public string password;

    public LoginRequest()
        : base(Operation.OperationCode.Login)
    {
    }

    public override void OnOperationResponse(Operation.ReturnCode returnCode)
    {
        if (returnCode == Operation.ReturnCode.Success)
        {
            SceneManager.LoadScene("MonoPoly1");
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
        byte[] bytes = SerializeHelper.Serialize<T>(obj);
        DoRequest(bytes);
    }
}