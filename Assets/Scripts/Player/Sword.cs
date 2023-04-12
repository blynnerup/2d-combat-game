using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimatorSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCd = .5f;
    
    private PlayerControls _playerControls;
    private Animator _animator;
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private PlayerController _playerController;
    private ActiveWeapon _activeWeapon;
    private Camera _camera;
    private bool _attackButtonDown, _isAttacking = false;

    private GameObject _slashAnimation;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();
        _activeWeapon = GetComponentInParent<ActiveWeapon>();
        weaponCollider.gameObject.SetActive(false);
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
        _playerControls.Combat.Attack.started += _ => StartAttacking();
        _playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update()
    {
        MouseFollowWithOffset();
        Attack();
    }

    private void StartAttacking()
    {
        _attackButtonDown = true;
    }

    private void StopAttacking()
    {
        _attackButtonDown = false;
    }

    private void Attack()
    {
        if (_attackButtonDown && !_isAttacking){
            // Fire the sword animation.
            _animator.SetTrigger(Attack1);
            weaponCollider.gameObject.SetActive(true);
            _isAttacking = true;
            _slashAnimation = Instantiate(slashAnimationPrefab, slashAnimatorSpawnPoint.position, Quaternion.identity);
            _slashAnimation.transform.parent = transform.parent;
            StartCoroutine(AttackCdRoutine());
        }
    }

    private IEnumerator AttackCdRoutine()
    {
        yield return new WaitForSeconds(swordAttackCd);
        _isAttacking = false;
    }

    public void DoneAttackingAnimationEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }
    
    public void SwingUpWithAnimation()
    {
        _slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (_playerController.FacingLeft)
        {
            _slashAnimation.GetComponent<SpriteRenderer>().flipX  = true;
        }
    }

    public void SwingDownFlipAnimation()
    {
        _slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (_playerController.FacingLeft)
        {
            _slashAnimation.GetComponent<SpriteRenderer>().flipX  = true;
        }
    }
    
    private void MouseFollowWithOffset()
    {
        var playerScreenPoint = _camera.WorldToScreenPoint(_playerController.transform.position);
        var mousePos = Input.mousePosition;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        _activeWeapon.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, angle) : Quaternion.Euler(0, 0, angle);
        weaponCollider.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
    }
}
