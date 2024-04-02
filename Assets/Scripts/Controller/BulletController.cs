using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[Serializable]
[RequireComponent(typeof (CapsuleCollider))]
public class BulletController : NetworkBehaviour
{
    public BulletSpawner parent;
    private NetworkObject _networkObject;
    private float _force;
    private Rigidbody _rb;
    private float _minY, _maxY;
    private Vector3 _direction;

    private void Awake()
    {
        _networkObject = GetComponent<NetworkObject>();
        _rb = GetComponent<Rigidbody>();
        _force = GameManager.Instance.BulletSpeed;
    }


    private void FixedUpdate()
    {
        _rb.velocity = _direction * _force;
    } 
    
    /*private void LateUpdate()
    { 
       Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

       if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
       {
           Debug.Log("Bullet Out");
           //transform.position = Vector3.zero;
           _rb.isKinematic = true;
           DisableServerRpc();
       }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Debug.Log("Bullet Out");
            _rb.isKinematic = true;
            DisableServerRpc();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (Camera.main.transform.rotation.z != 0)
        {
            _direction = -transform.up;
        }
        else
        {
            _direction = transform.up;
        }
    }
    public void Play()
    {
        //transform.position = position;
        _rb.isKinematic = false;
        gameObject.SetActive(true);
        //EnableServerRpc();
    }
    public void Play(Vector3 position)
    {
        transform.position = position;
        _rb.isKinematic = false;
        gameObject.SetActive(true);
        //EnableServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void EnableServerRpc()
    {
        Debug.Log("EnableServerRpc");
        gameObject.SetActive(true);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DisableServerRpc()
    {
        Debug.Log("DisableServerRpc");
        parent.Return(_networkObject);
    }
}
