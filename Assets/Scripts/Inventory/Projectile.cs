using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVfx;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;

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

    public void UpdatePorjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = col.gameObject.GetComponent<Indestructible>();
        PlayerHealth playerHealth = col.gameObject.GetComponent<PlayerHealth>();

        if (!col.isTrigger && (enemyHealth || indestructible || playerHealth))
        {
            var spawnTransform = transform;
            if ((playerHealth && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                playerHealth.TakeDamage(1, transform);
                Instantiate(particleOnHitPrefabVfx, spawnTransform.position, spawnTransform.rotation);    
                Destroy(gameObject);
            } else if (!col.isTrigger && indestructible)
            {
                Instantiate(particleOnHitPrefabVfx, spawnTransform.position, spawnTransform.rotation);    
                Destroy(gameObject);
            }
            
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, _startPosition) > projectileRange)
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
