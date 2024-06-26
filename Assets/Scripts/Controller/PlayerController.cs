using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Player playerName;
    [SerializeField] private Vector2 minmaxX;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private List<GameObject> lifeList;
    
    private BulletSpawner _bulletSpawner;
    private NetworkObject _networkObject;
    private SpriteRenderer _spriteRenderer;
    private float _moveSpeed;
    private float _shotDelay;
    private bool _isDelay;
    
    private enum Player
    {
        PlayerA,
        PlayerB
    }
    
    private void Awake()
    {
        _bulletSpawner = GameManager.Instance.BulletSpawner;
        _networkObject = GetComponent<NetworkObject>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _moveSpeed = GameManager.Instance.PlayerMoveSpeed;
        _shotDelay = GameManager.Instance.ShotDelay;
    }
    
    private void Initialize()
    {
        if (_networkObject.OwnerClientId == 0)
        {
            playerName = Player.PlayerA;
            transform.name = "PlayerA";
            _spriteRenderer.sprite = sprites[0];
            transform.rotation = Quaternion.Euler(0,0,0);
            transform.position = new Vector3(0f, -15f, 0f);
        }
        
        if (_networkObject.OwnerClientId >= 1)
        {
            playerName = Player.PlayerB;
            transform.name = "PlayerB";
            _spriteRenderer.sprite = sprites[1];
            transform.rotation = Quaternion.Euler(180,0,0);
            transform.position = new Vector3(0f, 18f, 0f);
        }
        
        if (_networkObject.IsOwner)
            enabled = true;
        else
            enabled = false;

        if (IsServer)
        {
            Camera.main.transform.rotation = Quaternion.identity;
        }
        else
        {
            Camera.main.transform.rotation = Quaternion.Euler(0f,0f,180f);
        }
    }
    //Owner일 때 위치 설정
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Start()
    {
       StartCoroutine(ShotDelay());
    }
    
    private void Update()
    {
        if (!IsOwner || !GameManager.Instance.IsGameStart || !Application.isFocused) return;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            Shooting();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.GetComponent<BulletController>().DisableBullet();
            UpdateLifeServerRpc("Hit");
        }
    }

    private void Move(Vector3 direction)
    {
        switch (playerName)
        {
            case Player.PlayerA:
                transform.position += direction * (_moveSpeed * Time.deltaTime);
                break;
            case Player.PlayerB:
                transform.position -= direction * (_moveSpeed * Time.deltaTime);
                break;
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minmaxX[0], minmaxX[1]),
            transform.position.y, transform.position.z);
    }
    
    private void Shooting()
    {
        if (_isDelay) return;
        
        _isDelay = true;
        
        _bulletSpawner.SpawnBullet(transform.position, _spriteRenderer.size, transform.up);
    }
    
    private void ApplyDamage()
    {
        int index;
        for(index = 0; index < lifeList.Count; index++)
        {
            if (lifeList[index].activeSelf)
            {
                lifeList[index].SetActive(false);
                break;
            }
        }
        
        if (index == 2)
            GameManager.Instance.GameEnd(playerName.ToString());
    }

    IEnumerator ShotDelay()
    {
        while (true)
        {
            if (_isDelay)
            {
                yield return new WaitForSeconds(_shotDelay);
                _isDelay = false;
            }

            yield return new WaitForEndOfFrame();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void UpdateLifeServerRpc(string message)
    {
        Debug.Log("ServerRpc - " + message);
        UpdateLifeClientRpc(message);
    } 

    [ClientRpc]
    public void UpdateLifeClientRpc(string message)
    {
        Debug.Log("ClientRpc - " + message);
        ApplyDamage();
    }
}
