using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    public void OnClickMacth()
    {
        Operation.NullMessage nullMessage = new Operation.NullMessage();
        PhotonEngine.Instance.DoRequest<Operation.NullMessage>(Operation.OperationCode.MatchRequest, nullMessage);

        lobbyRoot.SetActive(false);
        readyRoot.SetActive(true);
    }

    public void OnClickReady()
    {
        if (RoomSystem.Instance == null)
        {
            return;
        }
        Operation.ReadyData data = new Operation.ReadyData();
        data.roomID = RoomSystem.Instance.roomID;
        PhotonEngine.Instance.DoRequest<Operation.ReadyData>(Operation.OperationCode.Ready, data);
    }

    private void UpdateRoom()
    {
        if (RoomSystem.Instance == null)
        {
            return;
        }
        int i = 0;
        for (i = 0; RoomSystem.Instance.clients != null && i < RoomSystem.Instance.clients.Count; ++i)
        {
            var client = RoomSystem.Instance.clients[i];
            if (i < names.Length)
            {
                names[i].text = client.userName;
            }
            if (client.userName == PhotonEngine.Instance.UserName)
            {
                if (client.ready)
                {
                    readyButton.interactable = false;
                }
                else
                {
                    readyButton.interactable = true;
                }
            }
        }
        for (; i < names.Length; ++i)
        {
            names[i].text = "等待玩家";
        }
    }

    private void Update()
    {
        UpdateRoom();
    }

    public GameObject lobbyRoot;
    public GameObject readyRoot;

    public Button readyButton;
    public Text[] names;
}
