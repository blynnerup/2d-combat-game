using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private PlayerControls _playerControls;
    private Animator _animator;
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the "Attack Event" += pass nothing through it => function Event to subscribe to.
        _playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Attack()
    {
        // Fire the sword animation.
        _animator.SetTrigger(Attack1);
    }
}
