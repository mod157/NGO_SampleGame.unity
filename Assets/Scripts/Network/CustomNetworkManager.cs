using System;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
public class CustomNetworkManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    
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
    }

    private void OnClientConnected(ulong ClientId)
    {
        if (IsServer && _networkManager.ConnectedClientsIds.Count > 2)
        {
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
        _networkManager.Shutdown();
    }
    
    private void OnServerStarted()
    {
        Debug.Log($"Server Start");
    }
    
    private void OnServerStoped(bool isStop)
    {
        Debug.Log($"Server Stop");
    }
}
