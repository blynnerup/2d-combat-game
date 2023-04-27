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
    [SerializeField] private WeaponInfo weaponInfo;
    
    private Transform _weaponCollider;
    private Animator _animator;
    private static readonly int Attack1 = Animator.StringToHash("Attack");

    private GameObject _slashAnimation;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimatorSpawnPoint = GameObject.Find("SlashAnimationSpawnPoint").transform;
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {
        // Fire the sword animation.
        _animator.SetTrigger(Attack1);
        _weaponCollider.gameObject.SetActive(true);
        //_isAttacking = true;
        _slashAnimation = Instantiate(slashAnimationPrefab, slashAnimatorSpawnPoint.position, Quaternion.identity);
        _slashAnimation.transform.parent = transform.parent;
    }
    public void DoneAttackingAnimationEvent()
    {
        _weaponCollider.gameObject.SetActive(false);
    }
    
    public void SwingUpWithAnimation()
    {
        _slashAnimation.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            _slashAnimation.GetComponent<SpriteRenderer>().flipX  = true;
        }
    }

    public void SwingDownFlipAnimation()
    {
        _slashAnimation.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (PlayerController.Instance.FacingLeft)
        {
            _slashAnimation.GetComponent<SpriteRenderer>().flipX  = true;
        }
    }
    
    private void MouseFollowWithOffset()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        var mousePos = Input.mousePosition;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        ActiveWeapon.Instance.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, angle) : Quaternion.Euler(0, 0, angle);
        _weaponCollider.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, 0) : Quaternion.Euler(0, 0, 0);
    }
}
