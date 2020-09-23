using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Operation;

public class LoginHandler : Handler
{
    public string username;
    public string password;

    public LoginHandler()
        : base(OperationCode.Login)
    {
    }

    public override void OnEvent(byte[] data)
    {
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
        if (returnCode == Operation.ReturnCode.Success)
        {
            LoginSuccessResponse obj = PackageHelper.Desirialize<LoginSuccessResponse>(returnData);
            PhotonEngine.Instance.UserName = obj.userName;
            if (GameStateContext.Instance != null)
            {
                GameStateContext.Instance.UserName = obj.userName;
            }

            EventSystem.Dispatch(EventID.LoginSuccess);
        }
    }
}