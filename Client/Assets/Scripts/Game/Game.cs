using System;
using UnityEngine;
using Operation;

public class Game : MonoBehaviour
{
    public static Game Instance = null;

    public static bool handshake = false;
    private bool joined = false;

    public UISelectRace selectRace;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    
    public void Start()
    {
        
    }

    private void Update()
    {
        // 进入房间
        if (!joined)
        {
            if (handshake)
            {
                JoinRoom joinRoom = new JoinRoom();
                joinRoom.roomID = PhotonEngine.Instance.RoomID;
                joinRoom.userName = PhotonEngine.Instance.UserName;
                PhotonEngine.Instance.DoRequest(OperationCode.JoinRoom, joinRoom);

                joined = true;
            }
        }
    }
}