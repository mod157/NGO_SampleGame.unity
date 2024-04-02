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
    private float _minY, _maxY;
    private Vector3 _direction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _force = GameManager.Instance.BulletSpeed;
    }

    public override void OnNetworkSpawn()
    {
        _rb.isKinematic = false;
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.up * _force;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Wall")) return;
        //if (!NetworkManager.Singleton.IsServer) return;
        Debug.Log("Bullet Out");
        _rb.isKinematic = true;
        Debug.Assert(_networkObject != null);
        Debug.Assert(_prefab != null);
        NetworkObjectPool.Instance.ReturnNetworkObject(_networkObject, _prefab);
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ReturnServerRpc()
    {
        Debug.Log("Client -> Server Messsge : ReturnBullet");
        NetworkObjectPool.Instance.ReturnNetworkObject(_networkObject, _prefab);
    }

    public NetworkObject NetworkObject
    {
        set => _networkObject = value;
        get => _networkObject;
    }
    public GameObject Prefab
    {
        set => _prefab = value;
        get => _prefab;
    }
}
