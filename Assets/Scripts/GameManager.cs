using System;
using System.Collections;
using System.Collections.Generic;
using Robotry.Utils;
using Unity.Netcode;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("PlayerOption")]
    [SerializeField] private int playerLife = 3;
    [SerializeField] private float playerMoveSpeed = 100f;
    [SerializeField] private float bulletSpeed = 500f;
    [SerializeField] private float shotDelay = 0.25f;

    [SerializeField] private List<PlayerController> playerControllerList;
    

    private NetworkVariable<int> playerNum = new NetworkVariable<int>();

    private bool _isGameStart = false;

    private void Awake()
    {
        playerControllerList = new List<PlayerController>(2);
        StartCoroutine(GameReady());
    }

    private void Update()
    {
        if(NetworkManager.Singleton.IsServer)
            playerNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
    
    public void GameStart()
    {
        Debug.Log("Client 2 Game Start");
        _isGameStart = true;
    }
    
    public void GameEnd()
    {
        Debug.Log("GameEnd");
        _isGameStart = false;
    }
    
   

    

    private IEnumerator GameReady()
    {
        while (true)
        {
            if (playerNum.Value == 2 && !_isGameStart)
            {
                GameStart();
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    
    public int PlayerLife => playerLife;
    public float PlayerMoveSpeed => playerMoveSpeed;
    public float BulletSpeed => bulletSpeed;
    public float ShotDelay => shotDelay;
    
    public bool IsGameStart => _isGameStart;

    public int PlayerCount => playerNum.Value;
}
