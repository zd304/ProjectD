using System;
using System.Collections.Generic;
using Operation;

public class RoomHandler : Handler
{
    public RoomHandler() : base(OperationCode.RoomSync) { }

    public override void OnEvent(byte[] data)
    {
        RoomData roomData = PackageHelper.Desirialize<RoomData>(data);
        LobbyData.Instance.roomID = roomData.roomID;
        LobbyData.Instance.clients = roomData.clientInfos;
        LobbyData.Instance.dirty = true;
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
    }
}