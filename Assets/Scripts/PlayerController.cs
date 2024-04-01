using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerName playerName;
    
    private int _life;
    private float _moveSpeed;

    private void Start()
    {
        _life = GameManager.Instance.PlayerLife;
        _moveSpeed = GameManager.Instance.PlayerMoveSpeed;
    }

    private enum PlayerName
    {
        PlayerA,
        PlayerB
    }
    
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }
    }

    void Move(Vector3 direction)
    {
        switch (playerName)
        {
            case PlayerName.PlayerA:
                transform.position += direction * (_moveSpeed * Time.deltaTime);
                break;
            case PlayerName.PlayerB:
                
                transform.position -= direction * (_moveSpeed * Time.deltaTime);
                break;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            ApplyDamage();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
    }
    
    private void ApplyDamage()
    {
        _life -= 1;
        if (_life == 0)
        {
            GameManager.Instance.GameEnd();
        }
    }
}
