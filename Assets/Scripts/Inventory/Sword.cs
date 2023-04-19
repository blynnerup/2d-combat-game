using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Player;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimationPrefab;
    [SerializeField] private Transform slashAnimatorSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCd = .5f;
    
    private Animator _animator;
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private PlayerController _playerController;
    private ActiveWeapon _activeWeapon;

    private GameObject _slashAnimation;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponentInParent<PlayerController>();
        _activeWeapon = GetComponentInParent<ActiveWeapon>();
        weaponCollider.gameObject.SetActive(false);
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }


    public void Attack()
    {
        
            // Fire the sword animation.
            _animator.SetTrigger(Attack1);
            weaponCollider.gameObject.SetActive(true);
            //_isAttacking = true;
            _slashAnimation = Instantiate(slashAnimationPrefab, slashAnimatorSpawnPoint.position, Quaternion.identity);
            _slashAnimation.transform.parent = transform.parent;
            StartCoroutine(AttackCdRoutine());
    }

    private IEnumerator AttackCdRoutine()
    {
        yield return new WaitForSeconds(swordAttackCd);
        ActiveWeapon.Instance.ToggleIsAttacking(false);
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
        var playerScreenPoint = Camera.main.WorldToScreenPoint(_playerController.transform.position);
        var mousePos = Input.mousePosition;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        _activeWeapon.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, angle) : Quaternion.Euler(0, 0, angle);
        weaponCollider.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
    }
}
