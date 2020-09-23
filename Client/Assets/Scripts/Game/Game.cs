using System;
using UnityEngine;
using Operation;

public class Game : MonoBehaviour
{
    public static Game Instance = null;

    private GameStateSystem gameStateSystem = new GameStateSystem();

    public GameStateType defaultState = GameStateType.Login;

    public UISelectRace selectRace;

    public GameStateSystem GameStateSystem
    {
        get
        {
            return gameStateSystem;
        }
    }

    private void Awake()
    {
        Instance = this;

        GameObject.DontDestroyOnLoad(gameObject);

        gameStateSystem.Initialize();
    }

    private void OnDestroy()
    {
        gameStateSystem.Uninitialize();

        Instance = null;
    }

    
    public void Start()
    {
        gameStateSystem.SetState(defaultState);
    }

    private void Update()
    {
        gameStateSystem.OnRenderTick();
    }

    private void FixedUpdate()
    {
        gameStateSystem.OnLogicTick();
    }
}