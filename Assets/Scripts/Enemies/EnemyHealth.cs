using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVfxPrefab;
    [SerializeField] private float knockBackThrust = 15f;

    private int _currentHealth;
    private Knockback knockback;
    private Flashing _flashing;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        _flashing = GetComponent<Flashing>();
    }

    private void Start()
    {
        _currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        knockback.GetKnockedBack(PlayerController.Instance.transform, knockBackThrust);
        StartCoroutine(_flashing.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(_flashing.GetRestoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            Instantiate(deathVfxPrefab, transform.position, quaternion.identity);
            Destroy(gameObject);
        }
    }
}
