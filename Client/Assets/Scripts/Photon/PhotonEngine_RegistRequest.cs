using UnityEngine;
using ExitGames.Client.Photon;

public partial class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    private void RegistRequests()
    {
        RegistRequest(new LoginRequest());
        RegistRequest(new RegistRequest());
    }
}