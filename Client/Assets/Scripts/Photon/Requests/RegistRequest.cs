using System;
using System.Collections.Generic;
using Operation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegistRequest : Request
{
    public RegistRequest() : base(Operation.OperationCode.Regist) { }

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

    public override void OnOperationResponse(ReturnCode returnCode)
    {
        if (returnCode == Operation.ReturnCode.Success)
        {
            SceneManager.LoadScene("Login");
        }
    }
}