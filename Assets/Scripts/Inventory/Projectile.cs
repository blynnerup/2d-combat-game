using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVfx;

    private WeaponInfo _weaponInfo;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    public void UpdateWeaponInfo(WeaponInfo weaponInfo)
    {
        _weaponInfo = weaponInfo;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = col.gameObject.GetComponent<Indestructible>();

        if (enemyHealth || indestructible)
        {
            enemyHealth?.TakeDamage(_weaponInfo.weaponDamage);
            Destroy(gameObject);
            Instantiate(particleOnHitPrefabVfx, transform.position, transform.rotation);    
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, _startPosition) > _weaponInfo.weaponRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed));
    }

    private IEnumerator DelayDestroyRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        
    }
}
