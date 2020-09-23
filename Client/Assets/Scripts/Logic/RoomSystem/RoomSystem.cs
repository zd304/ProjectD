using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomSystem
{
    private static RoomSystem instance = null;
    public static RoomSystem Instance
    {
        get
        {
            return instance;
        }
    }

    public static RoomSystem Create(string userName)
    {
        if (instance != null)
        {
            Debug.LogError("RoomSystem Exist!");
            return instance;
        }
        instance = new RoomSystem();
        instance.userName = userName;
        return instance;
    }

    private RoomSystem() { }

    public void Destroy()
    {
        instance = null;
    }

    public string userName;
    public int roomID;
    public List<Operation.RoomClient> clients = null;
}