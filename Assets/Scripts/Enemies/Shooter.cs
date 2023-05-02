using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [Tooltip("Distance from enemy to spawn of bullet.")]
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts;
    [Tooltip("Time in between shots.")]
    [SerializeField] private float restTime = 1;
    [SerializeField] private bool stagger;
    [Tooltip("Stagger must be enabled in order to work properly.")]
    [SerializeField] private bool oscillate;

    private bool _isShooting = false;

    private void OnValidate()
    {
        if (oscillate)
        {
            stagger = true;
            
        }

        if (!oscillate)
        {
            stagger = false;
            
        }

        if (projectilesPerBurst < 1)
        {
            projectilesPerBurst = 1;
        }

        if (burstCount < 1)
        {
            burstCount = 1;
        }

        if (timeBetweenBursts < 0.1f)
        {
            timeBetweenBursts = 0.1f;
            
        }
        
        if (restTime < 0.1f)
        {
            restTime = 0.1f;
        }

        if (startingDistance < 0.1f)
        {
            startingDistance = 0.1f;
        }

        if (angleSpread == 0)
        {
            projectilesPerBurst = 1;
        }

        if (bulletMoveSpeed <= 0)
        {
            bulletMoveSpeed = 0.1f;
        }
    }

    public void Attack()
    {
        if (!_isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private IEnumerator ShootRoutine()
    {
        _isShooting = true;

        TargetConeOfInfluence(out var startAngle, out var currentAngle, out var angleStep, out var endAngle);
        float timeBetweenProjectiles = 0;
        
        if (stagger) {timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst;}

        for (var i = 0; i < burstCount; i++)
        {
            if(!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }

            switch (oscillate)
            {
                case true when i % 2 != 1:
                    TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
                    break;
                case true:
                    currentAngle = endAngle;
                    endAngle = startAngle;
                    startAngle = currentAngle;
                    angleStep *= -1;
                    break;
            }

            for (var j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);
                
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateProjectileSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;
                if(stagger) {yield return new WaitForSeconds(timeBetweenProjectiles);}
            }

            currentAngle = startAngle;

            if (!stagger) {yield return new WaitForSeconds(timeBetweenBursts);}
        }
        
        yield return new WaitForSeconds(restTime);
        _isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0;
        angleStep = 0;

        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        var position = transform.position;
        float x = position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        return new Vector2(x, y);
    }
}
