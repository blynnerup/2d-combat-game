using System;
using Inventory;
using Player;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;

    private Animator _animator;
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public void Attack()
    {
        _animator.SetTrigger(AttackHash);
    }
    
    private void MouseFollowWithOffset()
    {
        var playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);
        var mousePos = Input.mousePosition;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        
        ActiveWeapon.Instance.transform.rotation = mousePos.x < playerScreenPoint.x ? Quaternion.Euler(0, -180, angle) : Quaternion.Euler(0, 0, angle);
    }
    
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
    }
}
