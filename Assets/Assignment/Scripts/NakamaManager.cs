using UnityEngine;
using Nakama;
using System;

public class NakamaManager : MonoBehaviour
{
    public static NakamaManager Instance { get; private set; }

    [SerializeField] private string scheme = "http";
    [SerializeField] private string host = "127.0.0.1";
    [SerializeField] private int port = 7350;
    [SerializeField] private string serverKey = "defaultkey";

    private IClient client;
    private ISocket socket;
    private ISession session;
    private string matchId;

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitNakama();
    }

    private async void InitNakama ()
    {
        client = new Client(scheme, host, port, serverKey);
        session = await client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);

        socket = client.NewSocket();
        socket.Connected += () => Debug.Log("Socket connected.");
        socket.Closed += () => Debug.Log("Socket closed.");
        await socket.ConnectAsync(session);
        Debug.Log("Connected to Nakama server.");
    }

    public async void CreateMatch ()
    {
        var match = await socket.CreateMatchAsync();
        matchId = match.Id;
        Debug.Log("Match created with ID: " + match.Id);
    }

    public async void JoinMatch (string matchId)
    {
        try
        {
            var match = await socket.JoinMatchAsync(matchId);
            this.matchId = match.Id;
            Debug.Log("Joined match with ID: " + match.Id);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to join match: " + e.Message);
        }
    }

    public string GetMatchId () => matchId;

    public IClient GetClient () => client;
    public ISocket GetSocket () => socket;
    public ISession GetSession () => session;

    private void OnApplicationQuit ()
    {
        socket?.CloseAsync();
    }
}
