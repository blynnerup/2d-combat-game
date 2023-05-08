using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Enemies;
using Misc;
using Scene_Management;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider _healthSlider;
    private int _currentHealth;
    private bool _canTakeDamage = true;
    private Knockback _knockback;
    private Flashing _flashing;
    private const string HealthSliderText = "Health Slider";

    protected override void Awake()
    {
        base.Awake();
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

    public void HealDamage()
    {
        if (_currentHealth >= maxHealth) return;
        _currentHealth++;
        UpdateHealthSlider();
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
        UpdateHealthSlider();
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Debug.Log("Player died.");
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (_healthSlider == null)
        {
            _healthSlider = GameObject.Find(HealthSliderText).GetComponent<Slider>();
        }

        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = _currentHealth;
    }
}
