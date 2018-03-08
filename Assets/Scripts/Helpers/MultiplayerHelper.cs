using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class MultiplayerHelper
{
    private static bool isServerStarted = false;
    private static NetworkClient client;

    public static bool IsClientConnected { get { return client != null && client.isConnected; } }
    public static bool IsServerStarted { get { return isServerStarted; } }

    public static void SetupServer(int port = 4444)
    {
        NetworkServer.Listen(port);
        isServerStarted = true;
    }

    // Create a client and connect to the server port
    public static void SetupClient(string ip, int port = 4444)
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.Connect(ip, port);
    }
    
    // Create a local client and connect to the local server
    public static void SetupClientLocal()
    {
        client = ClientScene.ConnectLocalServer();
        client.RegisterHandler(MsgType.Connect, OnConnected);
    }


    private static void OnConnected(NetworkMessage netMsg)
    {
    }
}
