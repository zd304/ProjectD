using System;
using UnityEngine.SceneManagement;

public class RegistState : GameState
{
    public RegistState() : base(GameStateType.Regist)
    {
    }

    public override void OnEnter(GameStateContext context)
    {
        SceneManager.LoadScene("Regist");
        EventSystem.AddListener(EventID.BackLogin, OnBackLogin);
    }

    public override void OnExit(GameStateContext context)
    {
        EventSystem.RemoveListener(EventID.BackLogin, OnBackLogin);
    }

    public override void OnLogicTick(GameStateContext context)
    {
    }

    public override void OnRenderTick(GameStateContext context)
    {
    }

    private void OnBackLogin()
    {
        GameStateSystem.Instance?.SetState(GameStateType.Login);
    }
}