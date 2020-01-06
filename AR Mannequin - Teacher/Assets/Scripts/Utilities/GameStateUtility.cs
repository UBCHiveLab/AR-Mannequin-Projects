using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created by Kimberly Burke, 2019
/// 
/// Accessible save state data for the instance of the game
/// </summary>
public static class GameStateUtility
{
    private static bool connection;
    private static string roomName;

    public static void SetConnectionStatus(bool connected)
    {
        connection = connected;
    }

    public static bool GetConnectionStatus()
    {
        return connection;
    }

    public static void SetRoomName(string name)
    {
        roomName = name;
    }

    public static string GetRoomName()
    {
        return roomName;
    }
}
