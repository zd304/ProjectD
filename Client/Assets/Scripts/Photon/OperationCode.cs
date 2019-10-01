﻿using UnityEngine;
using ExitGames.Client.Photon;

namespace Operation
{
    public enum OperationCode : byte
    {
        Login,
        Regist,
    }
}

public partial class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{
    private void RegistRequests()
    {
        RegistRequest(new LoginRequest());
        RegistRequest(new RegistRequest());
    }
}