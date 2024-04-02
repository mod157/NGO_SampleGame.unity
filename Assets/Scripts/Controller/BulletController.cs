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

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rectTransform = GetComponent<RectTransform>();
        force = GameManager.Instance.BulletSpeed;
        _screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        Disable();
    }

   private void LateUpdate()
    {
        if (_screenPoint.x < 0 || _screenPoint.x > 1 || _screenPoint.y < 0 || _screenPoint.y > 1)
        {
            Debug.Log($"ScreenValue\n_sP: {_screenPoint.x}/{_screenPoint.y}");
            Debug.Log("Bullet Out");
            //_rectTransform.localPosition = Vector3.zero;
            //Disable();
        }
    }
    
    public void Shot(Vector3 position)
    {
        Debug.Log(name + " - Shooooooooting");
        _rectTransform.localPosition = position;
        Enable();
        _rb.velocity = Vector2.up * force;
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
