using System;
using System.Collections.Generic;
using Operation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegistHandler : Handler
{
    public RegistHandler() : base(Operation.OperationCode.Regist) { }

    public override void OnEvent(byte[] data)
    {
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
        if (returnCode == Operation.ReturnCode.Success)
        {
            EventSystem.Dispatch(EventID.BackLogin);
        }
    }
}