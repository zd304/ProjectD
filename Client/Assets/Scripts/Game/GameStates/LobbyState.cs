using System;
using UnityEngine.SceneManagement;
using Operation;

public class LobbyState : GameState
{
    public LobbyState()
       : base(GameStateType.Lobby)
    {
    }

    public override void OnEnter(GameStateContext context)
    {
        room = RoomSystem.Create(context.UserName);

        SceneManager.LoadScene("Lobby");

        EventSystem.AddListener(EventID.HandShake, OnHandShake);
    }

    public override void OnExit(GameStateContext context)
    {
        EventSystem.RemoveListener(EventID.HandShake, OnHandShake);

        room?.Destroy();
    }

    public override void OnLogicTick(GameStateContext context)
    {
    }

    public override void OnRenderTick(GameStateContext context)
    {
    }

    private void OnHandShake()
    {
        JoinRoom joinRoom = new JoinRoom();
        joinRoom.roomID = PhotonEngine.Instance.RoomID;
        joinRoom.userName = PhotonEngine.Instance.UserName;
        PhotonEngine.Instance.DoRequest(OperationCode.JoinRoom, joinRoom);
    }

    RoomSystem room = null;
}
