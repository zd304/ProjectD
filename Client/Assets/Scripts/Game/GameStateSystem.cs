using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameStateType
{
    Default,
    Login,
    Lobby,
    Regist,
    Loading,
}

public class GameStateSystem
{
    public static GameStateSystem Instance
    {
        get
        {
            if (Game.Instance != null)
            {
                return Game.Instance.GameStateSystem;
            }
            return null;
        }
    }

    public static GameStateContext GameContext
    {
        get
        {
            if (GameStateSystem.Instance != null)
            {
                return GameStateSystem.Instance.context;
            }
            return null;
        }
    }

    public void Initialize()
    {
        RegistState(GameStateType.Default, null);
        RegistState(new RegistState());
        RegistState(new LoginState());
        RegistState(new LobbyState());
    }

    public void Uninitialize()
    {
        if (currentState != null)
        {
            currentState.OnExit(context);
        }
        states.Clear();
    }

    private void RegistState(GameStateType stateType, GameState state)
    {
        if (states.ContainsKey(stateType))
        {
            Debug.LogError("重复注册游戏状态：" + state.Type.ToString());
            return;
        }
        states.Add(stateType, state);
    }

    private void RegistState(GameState state)
    {
        if (state == null)
        {
            return;
        }
        if (states.ContainsKey(state.Type))
        {
            Debug.LogError("重复注册游戏状态：" + state.Type.ToString());
            return;
        }
        states.Add(state.Type, state);
    }

    public void SetState(GameStateType stateType, bool forceEnter = false)
    {
        // 记录oldStateType防死循环
        GameState oldState = currentState;

        GameState newState = null;
        if (!states.TryGetValue(stateType, out newState))
        {
            return;
        }

        currentState = newState;

        if (oldState != currentState)
        {
            if (oldState != null)
            {
                oldState.OnExit(context);
            }
            if (newState != null)
            {
                newState.OnEnter(context);
            }
        }
    }

    public void OnRenderTick()
    {
        if (currentState != null)
        {
            currentState.OnRenderTick(context);
        }
    }

    public void OnLogicTick()
    {
        if (currentState != null)
        {
            currentState.OnLogicTick(context);
        }
    }

    public GameStateType CurrentGameStateType
    {
        get
        {
            if (currentState != null)
            {
                return currentState.Type;
            }
            return GameStateType.Default;
        }
    }

    public GameState currentState = null;
    public GameStateContext context = new GameStateContext();
    private Dictionary<GameStateType, GameState> states = new Dictionary<GameStateType, GameState>();
}