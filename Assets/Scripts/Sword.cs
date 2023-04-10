using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimatorSpawnPoint;

    private PlayerControls _playerControls;
    private Animator _animator;
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private PlayerController _playerController;
    private ActiveWeapon _activeWeapon;
    private Camera _camera;

    private GameObject slashAnimation;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();
        _activeWeapon = GetComponentInParent<ActiveWeapon>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        // Subscribe to the "Attack Event" += pass nothing through it => function Event to subscribe to.
        _playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    private void Attack()
    {
        // Fire the sword animation.
        _animator.SetTrigger(Attack1);

        slashAnimation = Instantiate(slashAnimationPrefab, slashAnimatorSpawnPoint.position, Quaternion.identity);
        slashAnimation.transform.parent = transform.parent;
    }

    public void SwingUpWithAnimation()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (_playerController.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX  = true;
        }
    }

    public void SwingDownFlipAnimation()
    {
        slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (_playerController.FacingLeft)
        {
            slashAnimation.GetComponent<SpriteRenderer>().flipX  = true;
        }
    }
    
    private void MouseFollowWithOffset()
    {
        var playerScreenPoint = _camera.WorldToScreenPoint(_playerController.transform.position);
        var mousePos = Input.mousePosition;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        _activeWeapon.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, angle) : Quaternion.Euler(0, 0, angle);
    }
}
