using UnityEngine;
using ExitGames.Client.Photon;

public partial class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    private void RegistHandlers()
    {
        RegistHandler(new LoginHandler());
        RegistHandler(new RegistHandler());
        RegistHandler(new RoomHandler());
        RegistHandler(new JoinGameServerHandler());

        RegistHandler(new JoinRoomHandler());
    }
}