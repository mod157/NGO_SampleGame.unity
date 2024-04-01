using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerName resolutionDropdown;
    
    private int _life;
    private float _moveSpeed;
    
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
        Debug.Log(Time.deltaTime);
        transform.position += direction * (_moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log(other.transform.parent.name);
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
