using System.Collections;
using System.Collections.Generic;
using Robotry.Utils;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int playerLife = 3;

    [SerializeField] private PlayerController playerA;
    [SerializeField] private PlayerController playerB;
    
    public void GameEnd()
    {
        Debug.Log("GameEnd");
    }
}
