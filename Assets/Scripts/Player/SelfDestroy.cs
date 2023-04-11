using System;
using UnityEngine;

namespace Player
{
    public class SelfDestroy : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (_particleSystem && !_particleSystem.IsAlive())
            {
                DestroySelf();
            }
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
