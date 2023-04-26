using System;
using System.Collections;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class ActiveWeapon : Scene_Management.Singleton<ActiveWeapon>
    {
        public MonoBehaviour CurrentActiveWeapon { get; private set; }
        
        private PlayerControls _playerControls;
        private bool _attackButtonDown, _isAttacking = false;
        private Camera _camera;
        private float _timeBetweenAttacks;

        protected override void Awake()
        {
            base.Awake();
        
            _playerControls = new PlayerControls();

        }
        
        // Start is called before the first frame update
        private void Start()
        {
            _camera = Camera.main;
            // Subscribe to the "Attack Event" += pass nothing through it => function Event to subscribe to.
            _playerControls.Combat.Attack.started += _ => StartAttacking();
            _playerControls.Combat.Attack.canceled += _ => StopAttacking();
            
            AttackCooldown();
        }

        private void Update()
        {
            Attack();

        }

        public void NewWeapon(MonoBehaviour newWeapon)
        {
            CurrentActiveWeapon = newWeapon;
            AttackCooldown();
            _timeBetweenAttacks = ((IWeapon)CurrentActiveWeapon).GetWeaponInfo().weaponCooldown;
        }

        public void WeaponNull()
        {
            CurrentActiveWeapon = null;
        }
        
        
        private void OnEnable()
        {
            _playerControls.Enable();
        }
        
        
        private void StartAttacking()
        {
            _attackButtonDown = true;
        }

        private void StopAttacking()
        {
            _attackButtonDown = false;
        }

        private void Attack()
        {
            if (_attackButtonDown && !_isAttacking && CurrentActiveWeapon)
            {
                AttackCooldown();
                ((IWeapon)CurrentActiveWeapon).Attack();
            }
        }

        private void AttackCooldown()
        {
            _isAttacking = true;
            StopAllCoroutines();
            StartCoroutine(TimeBetweenAttackingRoutine());
        }

        private IEnumerator TimeBetweenAttackingRoutine()
        {
            yield return new WaitForSeconds(_timeBetweenAttacks);
            _isAttacking = false;
        }
    }
}
