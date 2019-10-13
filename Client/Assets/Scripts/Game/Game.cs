using System;
using UnityEngine;
using Operation;

public class Game : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        
    }

    // 进入房间
    public void Start()
    {
        JoinRoom joinRoom = new JoinRoom();
        joinRoom.roomID = PhotonEngine.Instance.RoomID;
        joinRoom.userName = PhotonEngine.Instance.UserName;
        PhotonEngine.Instance.DoRequest(OperationCode.JoinRoom, joinRoom);
    }


}