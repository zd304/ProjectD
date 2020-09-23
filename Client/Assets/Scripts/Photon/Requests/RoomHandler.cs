using System;
using System.Collections.Generic;
using Operation;

public class RoomHandler : Handler
{
    public RoomHandler() : base(OperationCode.RoomSync) { }

    public override void OnEvent(byte[] data)
    {
        RoomData roomData = PackageHelper.Desirialize<RoomData>(data);

        if (RoomSystem.Instance != null)
        {
            RoomSystem.Instance.clients = roomData.clientInfos;
            RoomSystem.Instance.roomID = roomData.roomID;
        }
    }

    public override void OnOperationResponse(ReturnCode returnCode, byte[] returnData)
    {
    }
}