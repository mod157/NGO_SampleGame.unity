using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof (NetworkRigidbody2D))]
[RequireComponent(typeof (CircleCollider2D))]
public class BulletController : MonoBehaviour
{
    private float force;
    private Rigidbody2D _rb;
    private RectTransform _rectTransform;
    private Vector3 _screenPoint;
    private float _minY, _maxY;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rectTransform = GetComponent<RectTransform>();
        force = GameManager.Instance.BulletSpeed;
        _screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        MoveWithInBound();
        Disable();
    }

   private void LateUpdate()
   { 
       if (transform.position.y < _minY || transform.position.y > _maxY)
       { 
           Debug.Log("Bullet Out");
           _rectTransform.position = Vector3.zero;
           Disable();
       }
    }
    
    public void Shot(Vector3 position)
    {
        _rectTransform.localPosition = position;
        Enable();
        _rb.velocity = Vector2.up * force;
    }
    
    private void MoveWithInBound()
    {
        RectTransform canvasRect = transform.parent.GetComponent<RectTransform>();
        float canvasHeight = canvasRect.rect.height;

        _minY = 0;
        _maxY = canvasHeight;
        
    }

    private void Enable()
    {
        gameObject.SetActive(true);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
