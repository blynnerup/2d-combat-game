using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Enemies;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int _currentHealth;
    private bool _canTakeDamage = true;
    
    
    private Knockback _knockback;
    private Flashing _flashing;

    private void Awake()
    {
        _knockback = GetComponent<Knockback>();
        _flashing = GetComponent<Flashing>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EnemyAi enemyAi = collision.gameObject.GetComponent<EnemyAi>();

        if (enemyAi)
        {
            TakeDamage(1, collision.transform);
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!_canTakeDamage) return;
        
        ScreenShakeManager.Instance.ShakeScreen();
        _knockback.GetKnockedBack(hitTransform, knockbackThrustAmount);
        StartCoroutine(_flashing.FlashRoutine());
        _canTakeDamage = false;
        _currentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }
}
