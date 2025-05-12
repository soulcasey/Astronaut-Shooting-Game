using UnityEngine;
using System;
using BestHTTP.SocketIO3;

public class NetworkManager : SingletonBase<NetworkManager>
{
    private SocketManager manager;
    private Socket socket;
    private const string SERVER_URL = "http://localhost:3000";

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Debug.Log("Attempting to connect");

        manager = new SocketManager(new Uri(SERVER_URL));
        socket = manager.GetSocket();

        socket.On(SocketIOEventTypes.Connect, () =>
        {
            Debug.Log("Connected to the server!");
            socket.Emit("message", "Hello from Unity!");
        });

        socket.On(SocketIOEventTypes.Disconnect, () =>
        {
            Debug.Log("Server Disconnected");
        });

        socket.On("message", (string message) =>
        {
            Debug.Log(message);
        });
    }
}
