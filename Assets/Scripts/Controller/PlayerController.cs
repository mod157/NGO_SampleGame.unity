using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private ulong clientID;
    [SerializeField] private Player playerName;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private NetworkObject networkObject;

    private ClientNetworkTransform _clientNetworkTransform;
    private RectTransform _rectTransform;
    private Image _image;
    private int _life;
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
        _clientNetworkTransform = GetComponent<ClientNetworkTransform>();
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _life = GameManager.Instance.PlayerLife;
        _moveSpeed = GameManager.Instance.PlayerMoveSpeed;
        _shotDelay = GameManager.Instance.ShotDelay;

    }

    private void Start()
    {
       GameManager.Instance.AddController(this);

       StartCoroutine(ShotDelay());
    }
    
    private void Update()
    {
        if (GameManager.Instance.IsGameStart || !networkObject.IsOwner) return;
        
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            ApplyDamage();
        }
    }


    public override void OnNetworkSpawn()
    {
        if (networkObject.IsOwner)
        {
            playerName = Player.PlayerA;
            _image.sprite = sprites[0];
            
            _rectTransform.parent.name = "PlayerA";
            _rectTransform.anchorMin = new Vector2(0.5f, 0);
            _rectTransform.anchorMax = new Vector2(0.5f, 0);
            _rectTransform.pivot = new Vector2(0.5f, 0);
            
            _rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            playerName = Player.PlayerB;
            _image.sprite = sprites[1];
            
            _rectTransform.parent.name = "PlayerB";
            _rectTransform.anchorMin = new Vector2(0.5f, 1);
            _rectTransform.anchorMax = new Vector2(0.5f, 1);
            _rectTransform.pivot = new Vector2(0.5f, 0);
            _rectTransform.localRotation = Quaternion.Euler(180,0,0);
            
            _rectTransform.anchoredPosition = Vector2.zero;
            
            enabled = false;
        }
    }

    [ServerRpc]
    void UpdatePositionServerRpc(Player updatePlayer, Vector3 newPosition)
    {
    }

    [ClientRpc]
    void UpdatePositionClientRpc(Player updatePlayer, Vector3 newPosition)
    {
    }

    private void Move(Vector3 direction)
    {
        switch (playerName)
        {
            case Player.PlayerA:
                _rectTransform.position += direction * (_moveSpeed * Time.deltaTime);
                break;
            case Player.PlayerB:
                _rectTransform.position -= direction * (_moveSpeed * Time.deltaTime);
                break;
        }
    }
    
    private void Shooting()
    {
        if (_isDelay) return;
        _isDelay = true;
        
        Debug.Log(playerName + " - Shot");
        BulletController bulletController = GameManager.Instance.GetBullet();
        Vector2 objectSize = _rectTransform.sizeDelta;
        bulletController.Shot(_rectTransform.localPosition + new Vector3(0f, objectSize.y, 0f));
    }
    
    
    private void ApplyDamage()
    {
        _life -= 1;
        if (_life == 0)
        {
            GameManager.Instance.GameEnd();
        }
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
}
