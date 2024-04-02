using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof (CapsuleCollider))]
public class BulletController : NetworkBehaviour
{
    private float _force;
    private Rigidbody _rb;
    private float _minY, _maxY;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _force = GameManager.Instance.BulletSpeed;
        Disable();
    }

   private void LateUpdate()
   { 
       /*if (transform.position.y < _minY || transform.position.y > _maxY)
       { 
           Debug.Log("Bullet Out");
           _rectTransform.position = Vector3.zero;
           Disable();
       }*/
    }
    
    public void Shot(Vector3 position)
    {
        transform.position = position;
        Enable();
        _rb.velocity = Vector3.up * _force;
    }

    private void Enable()
    {
        gameObject.SetActive(true);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
