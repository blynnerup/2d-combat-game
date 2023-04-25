using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Player;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;

    private Animator _animator;
    private static readonly int Fire = Animator.StringToHash("Fire");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        _animator.SetTrigger(Fire);
        GameObject newArrow =
            Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateWeaponInfo(weaponInfo);
    }
    
    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }
}
