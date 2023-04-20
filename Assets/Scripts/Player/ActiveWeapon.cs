using System;
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
        }

        private void Update()
        {
            Attack();

        }

        public void NewWeapon(MonoBehaviour newWeapon)
        {
            CurrentActiveWeapon = newWeapon;
        }

        public void WeaponNull()
        {
            CurrentActiveWeapon = null;
        }

        public void ToggleIsAttacking(bool value)
        {
            _isAttacking = value;
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
            if (_attackButtonDown && !_isAttacking)
            {
                _isAttacking = true;
            
                (CurrentActiveWeapon as IWeapon).Attack();   
            }
        }
    }
}
