using System;
using System.Collections.Generic;
using Photon.SocketServer;
using Operation;

namespace GameServer
{
    public class RoomClient
    {
        public string userNamae;
        public GameClientPeer peer;
    }

    public class Room
    {
        public int roomID;
        public int maxClient;
        public List<string> userNames = new List<string>();
        public List<RoomClient> roomClients = new List<RoomClient>();

        public void JoinRoom(GameClientPeer peer, string userName)
        {
            if (!userNames.Contains(userName))
            {
                return;
            }
            userNames.Remove(userName);

            RoomClient client = new RoomClient();
            client.peer = peer;
            client.userNamae = userName;
            roomClients.Add(client);

            if (userNames.Count <= 0 && maxClient == roomClients.Count)
            {
                for (int i = 0; i < roomClients.Count; ++i)
                {
                    RoomClient roomClient = roomClients[i];
                    NullMessage message = new NullMessage();

                    roomClient.peer.SendEvent(OperationCode.JoinRoom, message);
                }
            }
        }
    }
}
