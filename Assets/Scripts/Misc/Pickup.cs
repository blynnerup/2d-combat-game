using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

public class Pickup : MonoBehaviour
{
    [SerializeField] private float pickUpDistance = 5;
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float accelerationRate = 3;

    private Vector3 _moveDir;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;

        if (Vector3.Distance(transform.position, playerPos) < pickUpDistance)
        {
            _moveDir = (playerPos - transform.position).normalized;
            moveSpeed += accelerationRate;
        }
        else
        {
            _moveDir = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody2D.velocity = _moveDir * (moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerController>())
        {
            Destroy(gameObject);
        }
    }
}
