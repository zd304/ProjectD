﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LobbyServer
{
    public class RoomClient
    {
        public bool ready;
        public ClientInfo client;
    }

    public class Room
    {
        public void AddClient(ClientInfo clientInfo)
        {
            clientInfo.roomID = roomID;
            RoomClient c = new RoomClient() { ready = false, client = clientInfo };
            clients.Add(clientInfo.userName, c);

            BroadcastRoom();
        }

        public void Ready(ClientInfo clientInfo)
        {
            RoomClient c = null;
            if (!clients.TryGetValue(clientInfo.userName, out c))
            {
                return;
            }
            c.ready = true;

            BroadcastRoom();

            if (clients.Count >= NormalRoomMaxClient)
            {
                foreach (var kv in clients)
                {
                    RoomClient client = kv.Value;
                    if (!client.ready)
                    {
                        return;
                    }
                }
                GameApplication application = GameApplication.Instance as GameApplication;
                if (application == null)
                {
                    return;
                }

                Operation.S2SCreateRoom roomData = new Operation.S2SCreateRoom();
                roomData.roomID = roomID;

                foreach (var kv in clients)
                {
                    RoomClient client = kv.Value;

                    Operation.S2SRoomMember member = new Operation.S2SRoomMember();
                    member.userName = client.client.userName;
                    roomData.members.Add(member);
                }
                var serverPeer = application.GetMinLoadingServerPeer();
                serverPeer.DoRequest<Operation.S2SCreateRoom>(Operation.S2SOperationCode.CreateRoom, roomData);
            }
        }

        public void RemoveClient(ClientInfo clientInfo)
        {
            clientInfo.roomID = 0;
            RoomClient c = null;
            if (!clients.TryGetValue(clientInfo.userName, out c))
            {
                return;
            }
            clients.Remove(clientInfo.userName);

            BroadcastRoom();

            if (clients.Count == 0)
            {
                GameApplication application = GameApplication.Instance as GameApplication;
                if (application != null)
                {
                    application.RemoveRoom(this);
                }
            }
        }

        private void BroadcastRoom()
        {
            Operation.RoomData roomData = new Operation.RoomData();
            roomData.roomID = roomID;
            foreach (var kv in clients)
            {
                RoomClient matchClient = kv.Value;
                Operation.RoomClient roomClient = new Operation.RoomClient();
                roomClient.userName = matchClient.client.userName;
                roomClient.ready = matchClient.ready;
                roomData.clientInfos.Add(roomClient);
            }

            foreach (var kv in clients)
            {
                RoomClient matchClient = kv.Value;
                matchClient.client.client.SendEvent<Operation.RoomData>(Operation.OperationCode.RoomSync, roomData);
            }
        }

        public bool IsFull
        {
            get
            {
                return clients.Count >= maxClient;
            }
        }

        public Dictionary<string, RoomClient> clients = new Dictionary<string, RoomClient>();
        public int roomID;
        public int maxClient;

        public static readonly int NormalRoomMaxClient = 2;
    }
}
