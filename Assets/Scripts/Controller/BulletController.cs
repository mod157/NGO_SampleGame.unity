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

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green);
    }

    void FixedUpdate()
    {
        _rb.velocity = transform.up * _force;
    } 
    
    private void LateUpdate()
    { 
       Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

       Debug.Log("Bullet Position - " + viewportPosition);
       if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
       {
           Debug.Log("Bullet Out");
           transform.position = Vector3.zero;
           Disable();
       }
    }
   
    public void Shot(Vector3 position)
    {
        transform.position = position;
        Enable();
        _rb.isKinematic = false;
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
