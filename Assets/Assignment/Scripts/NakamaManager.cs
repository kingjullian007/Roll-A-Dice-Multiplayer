using UnityEngine;
using Nakama;

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
    }

    private async void Start ()
    {
        client = new Client(scheme, host, port, serverKey);
        session = await client.AuthenticateDeviceAsync(SystemInfo.deviceUniqueIdentifier);
        socket = client.NewSocket();
        socket.Connected += () => Debug.Log("Socket connected.");
        socket.Closed += () => Debug.Log("Socket closed.");
        await socket.ConnectAsync(session);
        Debug.Log("Connected to Nakama server.");
    }

    public IClient GetClient () => client;
    public ISocket GetSocket () => socket;
    public ISession GetSession () => session;
}
