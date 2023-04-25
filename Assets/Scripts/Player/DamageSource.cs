using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using Player;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DamageSource : MonoBehaviour
{
    private int _damageAmount;
    private void Start()
    {
        MonoBehaviour currentActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        _damageAmount = ((IWeapon)currentActiveWeapon).GetWeaponInfo().weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<EnemyHealth>())
        {
            EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(_damageAmount);
        }
    }
}
