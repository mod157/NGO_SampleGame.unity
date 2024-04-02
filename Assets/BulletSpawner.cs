using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private Transform bulletParentTransform;
    [SerializeField] private List<BulletController> bulletControllerList;
    
    public BulletController GetBullet()
    {
        BulletController currentBulletController = null;
        foreach (var bullet in bulletControllerList)
        {
            if (!bullet.gameObject.activeSelf)
                currentBulletController = bullet;
        }

        if (currentBulletController == null)
        {
            currentBulletController = CreateBullet();
            bulletControllerList.Add(currentBulletController);
        }

        return currentBulletController;
    }
    
    private BulletController CreateBullet()
    {
        GameObject newBullet = Instantiate(bulletObj, bulletParentTransform);
        NetworkObject no = newBullet.GetComponent<NetworkObject>();
        no.Spawn(true);
        
        bool isParent = no.TrySetParent(bulletParentTransform);
        BulletController newBulletController = newBullet.GetComponent<BulletController>();
        return newBulletController;
    }
}
