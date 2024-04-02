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
    [SerializeField] private float bulletSpeed = 500f;
    [SerializeField] private float shotDelay = 0.25f;

    [SerializeField] private List<PlayerController> playerControllerList;
    
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private RectTransform bulletParentTransform;
    [SerializeField] private List<BulletController> bulletControllerList;

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
    }
    
    public void GameEnd()
    {
        Debug.Log("GameEnd");
    }

    public BulletController GetBullet()
    {
        BulletController currentBulletController = null;
        foreach (var bullet in bulletControllerList)
        {
            if (!bullet.gameObject.activeSelf)
                currentBulletController = bullet;
        }
        
        if(currentBulletController == null)
            currentBulletController = CreateBullet();

        return currentBulletController;
    }

    private BulletController CreateBullet()
    {
        GameObject newBullet = Instantiate(bulletObj, bulletParentTransform);
        BulletController newBulletController = newBullet.GetComponent<BulletController>();

        return newBulletController;
    }

    public int PlayerLife => playerLife;
    public float PlayerMoveSpeed => playerMoveSpeed;
    public float BulletSpeed => bulletSpeed;
    public float ShotDelay => shotDelay;


    public bool IsGameStart => _isGameStart;
}
