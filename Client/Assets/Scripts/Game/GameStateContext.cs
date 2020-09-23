using System;
using System.Collections.Generic;

public class GameStateContext
{
    public static GameStateContext Instance
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

    public string UserName;
}
