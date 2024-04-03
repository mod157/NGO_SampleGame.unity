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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _force = GameManager.Instance.BulletSpeed;
    }

    private void Start()
    {
        Debug.Log("--------------- Start");
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
        Debug.Log("Bullet Out");
        
        if (!IsServer)
            ReturnServerRpc();
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void ReturnServerRpc()
    {
        Debug.Log("Client -> Server Messsge : ReturnBullet");
        NetworkObjectPool.Instance.ReturnNetworkObject(_networkObject, _prefab);
        //ReturnClientRpc();
    }
    
    [ClientRpc]
    private void ReturnClientRpc()
    {
        Debug.Log("Server -> Client Messsge : ReturnBullet");
        gameObject.SetActive(false);
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
