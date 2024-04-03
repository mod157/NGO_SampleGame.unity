using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class CustomNetworkManager : MonoBehaviour
{
    [SerializeField] private string hostIP;
    [SerializeField] private ushort hostPort;
    private NetworkManager _networkManager;
    
    private int maxConnections = 2; // 최대 연결 가능한 클라이언트 수
    internal bool IsClient => _networkManager.IsClient;
    internal bool IsHost => _networkManager.IsHost;
    internal bool IsServer => _networkManager.IsServer;
    
    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();
        _networkManager.OnClientConnectedCallback += OnClientConnected;
        _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnServerStopped += OnServerStoped;
        
        
        GetComponent<UnityTransport>().SetConnectionData(
            hostIP,  // The IP address is a string
            (ushort)hostPort, // The port number is an unsigned short
            "0.0.0.0" // The server listen address is a string.
        );
    }
    
    private void OnClientConnected(ulong ClientId)
    {
        if (IsServer && GameManager.Instance.PlayerCount > maxConnections)
        {
            Debug.Log("MaxClient Disconnect - " + ClientId);
            _networkManager.DisconnectClient(ClientId);
        }

        if (IsClient)
        {
            Debug.Log($"Local client {ClientId} connected, waiting for other players...");
        }
        else
        {
            Debug.Log($"Remote client {ClientId} connected");
        }
    }

    private void OnClientDisconnected(ulong ClientId)
    {
        Debug.Log($"Client {ClientId} disconnected");
        if (!_networkManager.IsServer)
        {
            GameManager.Instance.Reset();
        }
        else
        {
            GameManager.Instance.Ready();
        }
        
    }
    
    private void OnServerStarted()
    {
        Debug.Log($"Server Start");
    }
    
    private void OnServerStoped(bool isStop)
    {
        Debug.Log($"Server Stop");
        if(IsServer)
            ResetServerRpc();
    }

    [ServerRpc]
    private void ResetServerRpc()
    {
        ResetClientRpc();
    }

    [ClientRpc]
    private void ResetClientRpc()
    {
        GameManager.Instance.Reset();
    }
}
