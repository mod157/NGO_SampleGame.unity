using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject bulletObj;
    private Vector3 _bulletPosition;
    private NetworkObject _returnNetworkObject;
    public void Shot(Vector3 newPosition)
    {
        _bulletPosition = newPosition;
        /*int index = -1;
        for (int i = 0; i < spawnedBulletList.Count; i++)
        {
            if (!spawnedBulletList[i].activeSelf)
            {
                index = i;
                UpdatePositionServerRpc(i);
                break;
            }
        }

        if (index == -1)
        {
           ShotServerRpc();
        }*/

        ShotServerRpc();
        
    }

    public void Return(NetworkObject networkObject)
    {
        _returnNetworkObject = networkObject;
        ReturnBulletServerRpc();
    }
    
    private BulletController CreateBullet()
    {
        GameObject newBullet = Instantiate(bulletObj);
        NetworkObject no = newBullet.GetComponent<NetworkObject>();
        no.Spawn(true);
        
        BulletController newBulletController = newBullet.GetComponent<BulletController>();
        return newBulletController;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShotServerRpc()
    {
        NetworkObject poolObject = NetworkObjectPool.Instance.GetNetworkObject(bulletObj, _bulletPosition, Quaternion.identity);
        BulletController bulletController = poolObject.GetComponent<BulletController>();
        bulletController.parent = this;
        bulletController.Play();
        /* GameObject newBullet = Instantiate(bulletObj);
         spawnedBulletList.Add(newBullet);

         BulletController newBulletController = newBullet.GetComponent<BulletController>();
         newBulletController.Play(bulletPosition);
         newBulletController.parent = this;

         NetworkObject no = newBullet.GetComponent<NetworkObject>();
         no.Spawn(true);*/
    }
    [ServerRpc(RequireOwnership = false)]
    public void ReturnBulletServerRpc()
    {
        if (_returnNetworkObject != null)
        {
            NetworkObjectPool.Instance.ReturnNetworkObject(_returnNetworkObject, bulletObj);
            _returnNetworkObject = null;
        }
    }
    /*
    /*
    [ServerRpc]
    private void UpdatePositionServerRpc(int index)
    {
        BulletController selectBulletController = spawnedBulletList[index].GetComponent<BulletController>();
        selectBulletController.Play(_bulletPosition);
        NetworkObject no = selectBulletController.GetComponent<NetworkObject>();
        no.Spawn(true);
    }*/
}
