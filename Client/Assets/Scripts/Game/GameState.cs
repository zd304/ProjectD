using System;

public abstract class GameState
{
    public GameState(GameStateType type) { this.type = type; }
    /// <summary>
    /// 当进入状态的时候调用，一般用于初始化UI、注册事件
    /// </summary>
    /// <param name="context">状态机上下文</param>
    public abstract void OnEnter(GameStateContext context);

    /// <summary>
    /// 当游戏进入主循环时候调用
    /// </summary>
    /// <param name="context">状态机上下文</param>
    public abstract void OnRenderTick(GameStateContext context);

    /// <summary>
    /// 当游戏进入主循环时候调用
    /// </summary>
    /// <param name="context">状态机上下文</param>
    public abstract void OnLogicTick(GameStateContext context);

    /// <summary>
    /// 当退出状态的时候调用，一般用于销毁UI、清除事件、回收资源
    /// </summary>
    /// <param name="context">状态机上下文</param>
    public abstract void OnExit(GameStateContext context);

    protected GameStateType type = GameStateType.Default;
    public GameStateType Type
    {
        get
        {
            return type;
        }
    }
}