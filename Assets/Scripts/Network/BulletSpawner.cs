using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject bulletObj;
    private Vector3 _bulletPosition;
    private NetworkObject _returnNetworkObject;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnBulletStart;
    }

    private void SpawnBulletStart()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnBulletStart;
    }

    public void SpawnBullet(Vector3 playerPosition, Vector3 addPosition, Vector3 direction)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            SpawnServerRpc( new Vector3(playerPosition.x, playerPosition.y - addPosition.y, playerPosition.z), direction);
            return;
        }

        SpawnBullet(new Vector3(playerPosition.x, playerPosition.y + addPosition.y, playerPosition.z), direction);
    }

    private void SpawnBullet(Vector3 newPosition, Vector3 direction)
    {
        NetworkObject poolObject = NetworkObjectPool.Instance.GetNetworkObject(bulletObj, newPosition, Quaternion.identity);
        BulletController bulletController = poolObject.GetComponent<BulletController>();
        bulletController.Direction = direction;
        bulletController.SetNetworkObject = poolObject;
        bulletController.SetPrefab = bulletObj;
        if (!poolObject.IsSpawned) poolObject.Spawn(true);
    }
    

    [ServerRpc(RequireOwnership = false)]
    private void SpawnServerRpc(Vector3 newPosition, Vector3 direction)
    {
        Debug.Log("Client -> Server Messsge : SpawnBullet");
        SpawnBullet(newPosition, direction);
    }
}
