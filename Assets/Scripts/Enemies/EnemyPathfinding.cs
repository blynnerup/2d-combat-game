using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _moveDirection;
    private Knockback _knockback;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _knockback = GetComponent<Knockback>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if(_knockback.GettingKnockedBack) {return;}
        _rigidbody2D.MovePosition(_rigidbody2D.position + _moveDirection * (moveSpeed * Time.fixedDeltaTime));

        if (_moveDirection.x < 0)
        {
            _spriteRenderer.flipX = true;
        } else if (_moveDirection.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }

    public void MoveTo(Vector2 targetPosition)
    {
        _moveDirection = targetPosition;
    }

    public void StopMoving()
    {
        _moveDirection = Vector3.zero;
    }
}
