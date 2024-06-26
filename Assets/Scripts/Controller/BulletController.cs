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
    [SerializeField] private NetworkObject _networkObject;
    [SerializeField] private GameObject _prefab;
    private float _force;
    private Rigidbody _rb;
    private Vector3 _direction;
    private Vector2 _minmaxY;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _force = GameManager.Instance.BulletSpeed;
        _minmaxY = new Vector2(-25f, 25f);
    }

    private void Update()
    {
        if (!IsServer) return;
        if (transform.position.y < _minmaxY[0] || transform.position.y > _minmaxY[1])
        {
            DisableBullet();
        }
    }

    private void FixedUpdate()
    {
        if (_rb.isKinematic)
            _rb.isKinematic = false;
        
        _rb.velocity = _direction * _force;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Wall")) return;
        if (!IsServer) return;
        Debug.Log("Bullet Out");

        DisableBullet();
    }

    public void DisableBullet()
    {
        if(IsServer)
            _networkObject.Despawn();
    }
    
    public NetworkObject SetNetworkObject
    {
        set => _networkObject = value;
    }
    
    public GameObject SetPrefab
    {
        set => _prefab = value;
    }

    public Vector3 Direction
    {
        set => _direction = value;
    }
}
