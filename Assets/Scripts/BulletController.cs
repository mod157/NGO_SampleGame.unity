using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
[RequireComponent(typeof (CircleCollider2D))]
public class BulletController : MonoBehaviour
{
    private float force;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        force = GameManager.Instance.BulletSpeed;
    }

    
    
    private void LateUpdate()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);

        if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
        {
            (transform as RectTransform).localPosition = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
    
    private void Shot()
    {
        Debug.Assert(force == GameManager.Instance.BulletSpeed);
        gameObject.SetActive(true);
        _rb.velocity = Vector2.up * force;
    }
}
