using System;
using System.Collections;
using System.Collections.Generic;
using Robotry.Utils;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("PlayerOption")]
    [SerializeField] private int playerLife = 3;
    [SerializeField] private float playerMoveSpeed = 100f;
    [SerializeField] private float bulletSpeed = 100f;

    [SerializeField] private List<PlayerController> playerControllerList;

    private bool _isGameStart = false;

    private void Awake()
    {
        playerControllerList = new List<PlayerController>(2);
    }

    public void AddController(PlayerController pc)
    {
        playerControllerList.Add(pc);
        
        if(playerControllerList.Count == 2)
            GameStart();
    }
    
    public void GameStart()
    {
        Debug.Log("Client 2 Game Start");
        foreach (var controller in playerControllerList)
            controller.Initialized();
    }
    
    public void GameEnd()
    {
        Debug.Log("GameEnd");
    }

    public int PlayerLife => playerLife;
    public float PlayerMoveSpeed => playerMoveSpeed;
    public float BulletSpeed =>bulletSpeed;


    public bool IsGameStart => _isGameStart;
}
