using System;
using System.Collections.Generic;

public class LoginState : GameState
{
    public LoginState()
       : base(GameStateType.Login)
    {

    }

    public override void OnEnter(GameStateContext context)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        EventSystem.AddListener(EventID.LoginSuccess, OnLoginSuccess);
        EventSystem.AddListener(EventID.ClickRegist, OnClickRegist);
    }

    public override void OnExit(GameStateContext context)
    {
        EventSystem.RemoveListener(EventID.LoginSuccess, OnLoginSuccess);
        EventSystem.RemoveListener(EventID.ClickRegist, OnClickRegist);
    }

    public override void OnLogicTick(GameStateContext context)
    {
    }

    public override void OnRenderTick(GameStateContext context)
    {
    }

    private void OnLoginSuccess()
    {
        GameStateSystem.Instance.SetState(GameStateType.Lobby);
    }

    private void OnClickRegist()
    {
        GameStateSystem.Instance.SetState(GameStateType.Regist);
    }
}