using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public partial class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    private static PhotonEngine instance = null;

    PhotonPeer peer;
    Dictionary<Operation.OperationCode, Request> requests = new Dictionary<Operation.OperationCode, Request>();

    public static PhotonEngine Instance
    {
        get
        {
            return instance;
        }
    }

    public PhotonPeer Peer
    {
        get
        {
            return peer;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void OnDestroy()
    {
        instance = null;
    }

    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);

        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect("127.0.0.1:5055", "LobbyServer");

        RegistRequests();
    }

    public void DoRequest<T>(Operation.OperationCode opCode, T obj)
    {
        Request request;
        if (!requests.TryGetValue(opCode, out request))
        {
            return;
        }
        request.DoRequest<T>(obj);
    }

    public void DoRequest(Operation.OperationCode opCode, byte[] bytes)
    {
        Request request;
        if (!requests.TryGetValue(opCode, out request))
        {
            return;
        }
        request.DoRequest(bytes);
    }

    void Update()
    {
        peer.Service();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnEvent(EventData eventData)
    {
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Request request;
        if (!requests.TryGetValue((Operation.OperationCode)operationResponse.OperationCode, out request))
        {
            return;
        }
        Operation.ReturnCode returnCode = (Operation.ReturnCode)operationResponse.ReturnCode;
        request.OnOperationResponse(returnCode);
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        switch (statusCode)
        {
            case StatusCode.Connect:
                Debug.Log("链接服务器成功");
                break;
            case StatusCode.Disconnect:
                Debug.Log("断开服务器");
                break;
        }
    }

    private void RegistRequest(Request request)
    {
        requests.Add(request.OpCode, request);
    }

    private void UnregistRequest(Operation.OperationCode opCode)
    {
        requests.Remove(opCode);
    }
}
