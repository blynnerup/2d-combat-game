using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _moveDirection;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _moveDirection * (moveSpeed * Time.fixedDeltaTime));    
    }

    public void MoveTo(Vector2 targetPosition)
    {
        _moveDirection = targetPosition;
    }
}
