using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private ulong clientID;
    [SerializeField]
    private Player playerName;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private NetworkObject networkObject;

    private RectTransform _rectTransform;
    private Image _image;
    private int _life;
    private float _moveSpeed;
    
    
    private enum Player
    {
        PlayerA,
        PlayerB
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _life = GameManager.Instance.PlayerLife;
        _moveSpeed = GameManager.Instance.PlayerMoveSpeed;
        
    }

    private void Start()
    {
       GameManager.Instance.AddController(this);
    }
    
    void Update()
    {
        //Position Error 발생 시 Reset

        if (_rectTransform.anchoredPosition.y != 0)
        {
            Debug.Log(playerName+ "_"+ _rectTransform.anchoredPosition);
            _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, 0);
        }

        if (GameManager.Instance.IsGameStart || !networkObject.IsOwner) return;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }
        
        
    }

    public void Initialized()
    {
        Debug.Log($"initialized {networkObject.IsOwner}/{networkObject.OwnerClientId}/{networkObject.NetworkObjectId}/{networkObject.IsOwnedByServer}");
        if (networkObject.IsOwner)
        {
            playerName = Player.PlayerA;
            _image.sprite = sprites[0];
            
            _rectTransform.parent.name = "PlayerA";
            _rectTransform.anchorMin = new Vector2(0.5f, 0);
            _rectTransform.anchorMax = new Vector2(0.5f, 0);
            
        }
        else
        {
            playerName = Player.PlayerB;
            _image.sprite = sprites[1];
            
            _rectTransform.parent.name = "PlayerB";
            _rectTransform.anchorMin = new Vector2(0.5f, 1);
            _rectTransform.anchorMax = new Vector2(0.5f, 1);
            _rectTransform.localRotation = Quaternion.Euler(180,0,0);
        }

        UpdatePositionClientRpc(Player.PlayerB, Vector3.zero);
    }
    
    [ServerRpc]
    void UpdatePositionServerRpc(Vector3 newPosition)
    {
        // 서버에서 위치 업데이트를 호출하여 모든 클라이언트에게 전달
        UpdatePositionClientRpc(Player.PlayerB, newPosition);
    }

    [ClientRpc]
    void UpdatePositionClientRpc(Player updatePlayer, Vector3 newPosition)
    {
        if (updatePlayer == playerName)
        {
            _rectTransform.anchoredPosition = newPosition;
        }
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
        
        UpdatePositionClientRpc(playerName, _rectTransform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            ApplyDamage();
        }
    }
    
    private void ApplyDamage()
    {
        _life -= 1;
        if (_life == 0)
        {
            GameManager.Instance.GameEnd();
        }
    }
}
