using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyData
{
    private static LobbyData instance = null;
    public static LobbyData Instance
    {
        get
        {
            return instance;
        }
    }

    public static void Start()
    {
        instance = new LobbyData();
    }

    public static void Destroy()
    {
        instance = null;
    }

    public bool dirty = false;
    public int roomID;
    public List<Operation.RoomClient> clients = null;
}

public class UILobby : MonoBehaviour
{
    public void OnClickMacth()
    {
        LobbyData.Start();
        Operation.NullMessage nullMessage = new Operation.NullMessage();
        PhotonEngine.Instance.DoRequest<Operation.NullMessage>(Operation.OperationCode.MatchRequest, nullMessage);

        lobbyRoot.SetActive(false);
        readyRoot.SetActive(true);
    }

    public void OnClickReady()
    {
        Operation.ReadyData data = new Operation.ReadyData();
        data.roomID = LobbyData.Instance.roomID;
        PhotonEngine.Instance.DoRequest<Operation.ReadyData>(Operation.OperationCode.Ready, data);
    }

    private void Update()
    {
        if (LobbyData.Instance != null && LobbyData.Instance.dirty)
        {
            int i = 0;
            for (i = 0; i < LobbyData.Instance.clients.Count; ++i)
            {
                var client = LobbyData.Instance.clients[i];
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
    }

    public GameObject lobbyRoot;
    public GameObject readyRoot;

    public Button readyButton;
    public Text[] names;
}
