using System;
using System.Collections;
using System.Collections.Generic;
using Robotry.Utils;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Componet")] 
    [SerializeField] private UIManager uiManager;
    [SerializeField] private BulletSpawner bulletSpawner;
    [Space(15)]
    
    [Header("PlayerOption")]
    [SerializeField] private float playerMoveSpeed = 50f;
    [SerializeField] private float bulletSpeed = 50f;
    [SerializeField] private float shotDelay = 0.25f;
    private NetworkVariable<int> playerNum = new NetworkVariable<int>();

    private bool _isGameStart = false;

    private void Awake()
    {
        StartCoroutine(GameReady());
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            playerNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
        }
    }
    
    public void GameStart()
    {
        Debug.Log("Client 2 Game Start");
        _isGameStart = true;
    }
    
    public void GameEnd()
    {
        Debug.Log("GameEnd");
        Reset();
    }

    private IEnumerator GameReady()
    {
        while (true)
        {
            if (playerNum.Value == 2 && !_isGameStart)
            {
                uiManager.Progress.IsExit = true;
                yield return new WaitForSeconds(0.5f);
                GameStart();
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void Ready()
    {
        uiManager.ReadyConnected();
        StartCoroutine(GameReady());
        
        _isGameStart = false;
    }
    
    public void Reset()
    {
        _isGameStart = false;
        uiManager.ResetUI();
        StartCoroutine(GameReady());
    }
    
    public float PlayerMoveSpeed => playerMoveSpeed;
    public float BulletSpeed => bulletSpeed;
    public float ShotDelay => shotDelay;
    public bool IsGameStart => _isGameStart;
    public int PlayerCount => playerNum.Value;
    public BulletSpawner BulletSpawner => bulletSpawner;

   
}
