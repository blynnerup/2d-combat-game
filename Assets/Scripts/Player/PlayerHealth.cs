using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Enemies;
using Misc;
using Player;
using Scene_Management;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealth : Singleton<PlayerHealth>
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockbackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    public bool IsDead { get; private set; }
    
    private Slider _healthSlider;
    private int _currentHealth;
    private bool _canTakeDamage = true;
    private Knockback _knockback;
    private Flashing _flashing;
    private const string HealthSliderText = "Health Slider";
    private readonly int _deathHash = Animator.StringToHash("Death");
    private const string _townSceneText = "Town";

    protected override void Awake()
    {
        base.Awake();
        _knockback = GetComponent<Knockback>();
        _flashing = GetComponent<Flashing>();
    }

    private void Start()
    {
        IsDead = false;
        _currentHealth = maxHealth;
        
        UpdateHealthSlider();
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
        if (_currentHealth > 0 && !IsDead) return;
        IsDead = true;
        Destroy(ActiveWeapon.Instance.gameObject);
        _currentHealth = 0;
        GetComponent<Animator>().SetTrigger(_deathHash);
        StartCoroutine(DeathLoadSceneRoutine());
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        _canTakeDamage = true;
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        Stamina.Instance.ReplenishStaminaOnDeath();
        SceneManager.LoadScene(_townSceneText);
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
