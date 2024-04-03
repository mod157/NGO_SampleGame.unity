using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Robotry.Utils.Singleton<GameManager>
{
    private const string GAMEOVER_MESSAGE_PLAYER1 = "Winner <color=red>Red</color>";
    private const string GAMEOVER_MESSAGE_PLAYER2 = "Winner <color=blue>Blue</color>";
    
    [Header("Componet")] 
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BulletSpawner bulletSpawner;
    [Space(15)]
    
    [Header("PlayerOption")]
    [SerializeField] private float playerMoveSpeed = 50f;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private float shotDelay = 0.25f;

    [SerializeField]
    private bool _isGameStart = false;

    private int _playerCount = 0;

    private void Awake()
    {
        StartCoroutine(GameReady());
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            PlayerCountServerRpc(NetworkManager.Singleton.ConnectedClients.Count);
        }
    }
    
    public void GameStart()
    {
        Debug.Log("Client 2 Game Start");
        _isGameStart = true;
    }
    
    public void GameEnd(string text)
    {
        Debug.Log("GameEnd");
        _isGameStart = false;
        switch (text)
        {
            case "PlayerA":
                uiManager.GameEndUI(GAMEOVER_MESSAGE_PLAYER2, Shutdown);
                break;
            case "PlayerB":
                uiManager.GameEndUI(GAMEOVER_MESSAGE_PLAYER1, Shutdown);
                break;
        }
        
    }

    private void Shutdown()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }

    public void Ready()
    {
        Debug.Log("GM Ready");
        _isGameStart = false;
        uiManager.ReadyConnectedUI();
        StartCoroutine(GameReady());
    }
    
    public void Reset()
    {
        Debug.Log("GM Reset");
        _playerCount = 0;
        _isGameStart = false;
        Debug.Log("PC - " + PlayerCount);
        uiManager.ResetUI();
        StartCoroutine(GameReady());
    }
    
    private IEnumerator GameReady()
    {
        while (true)
        {
            if (_playerCount == 2 && !_isGameStart)
            {
                GameStart();
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (!IsServer)
        {
        }
        //NetworkManager.Singleton;
    }

    [ServerRpc]
    private void PlayerCountServerRpc(int count)
    {
        PlayerCountClientRpc(count);
    }

    [ClientRpc]
    private void PlayerCountClientRpc(int count)
    {
        _playerCount = count;
    }
    
    public float PlayerMoveSpeed => playerMoveSpeed;
    public float BulletSpeed => bulletSpeed;
    public float ShotDelay => shotDelay;
    public bool IsGameStart => _isGameStart;
    public int PlayerCount => _playerCount;
    public BulletSpawner BulletSpawner => bulletSpawner;

   
}
