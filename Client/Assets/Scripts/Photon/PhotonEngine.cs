using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public partial class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    private static PhotonEngine instance = null;

    PhotonPeer peer;
    Dictionary<Operation.OperationCode, Handler> handlers = new Dictionary<Operation.OperationCode, Handler>();

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

    public string UserName
    {
        set;
        get;
    }

    public int RoomID
    {
        set;
        get;
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

    void OnApplicationQuit()
    {
        peer.Disconnect();
    }

    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);

        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        peer.Connect("127.0.0.1:5055", "LobbyServer");

        RegistHandlers();
    }

    public void DoRequest<T>(Operation.OperationCode opCode, T obj)
    {
        byte[] data = PackageHelper.Serialize<T>(obj);

        Dictionary<byte, object> customParameters = new Dictionary<byte, object>();
        customParameters[0] = data;
        PhotonEngine.Instance.Peer.OpCustom((byte)opCode, customParameters, true);
    }

    public void DoRequest(Operation.OperationCode opCode, byte[] bytes)
    {
        Dictionary<byte, object> customParameters = new Dictionary<byte, object>();
        customParameters[0] = bytes;
        PhotonEngine.Instance.Peer.OpCustom((byte)opCode, customParameters, true);
    }

    public void Reconnect(string ip, string port, string app)
    {
        peer.Disconnect();
        string ipAndPort = ip + ":" + port;
        Debug.Log("重连服务器：" + ipAndPort + "，应用名：" + app);
        peer.Connect(ipAndPort, app);
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
        Handler handler;
        if (!handlers.TryGetValue((Operation.OperationCode)eventData.Code, out handler))
        {
            return;
        }
        byte[] bytes = null;
        object o;
        if (eventData.Parameters.TryGetValue(0, out o))
        {
            bytes = o as byte[];
        }
        handler.OnEvent(bytes);
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Handler handler;
        if (!handlers.TryGetValue((Operation.OperationCode)operationResponse.OperationCode, out handler))
        {
            return;
        }
        Operation.ReturnCode returnCode = (Operation.ReturnCode)operationResponse.ReturnCode;
        byte[] paramBytes = null;
        object param0;
        if (operationResponse.Parameters.TryGetValue(0, out param0))
        {
            paramBytes = param0 as byte[];
        }

        handler.OnOperationResponse(returnCode, paramBytes);
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

    private void RegistHandler(Handler request)
    {
        handlers.Add(request.OpCode, request);
    }

    private void UnregistRequest(Operation.OperationCode opCode)
    {
        handlers.Remove(opCode);
    }
}
