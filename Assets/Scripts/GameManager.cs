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
    [Space(10)]
    
    [SerializeField] private PlayerController playerA;
    [SerializeField] private PlayerController playerB;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           // Shot();
        }
    }
    
    public void GameEnd()
    {
        Debug.Log("GameEnd");
    }

    public int PlayerLife => playerLife;
    public float PlayerMoveSpeed => playerMoveSpeed;
    public float BulletSpeed =>bulletSpeed;


}
