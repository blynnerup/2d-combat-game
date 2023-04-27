using System.Collections;
using Scene_Management;
using UnityEngine;

namespace Player
{
    public class PlayerController : Singleton<PlayerController>
    {
        public bool FacingLeft
        {
            get => _facingLeft;
            set => _facingLeft = value;
        }
    
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float dashSpeed = 4f;
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private Transform weaponCollider;

        private PlayerControls _playerControls;
        private Vector2 _movement;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private static readonly int MoveX = Animator.StringToHash("moveX");
        private static readonly int MoveY = Animator.StringToHash("moveY");
        private Camera _camera;
        private SpriteRenderer _spriteRenderer;
        private Knockback _knockback;
        private float _startingMoveSpeed;

        private bool _facingLeft = false;
        private bool _isDashing = false;

        private void Start()
        {
            _camera = Camera.main;
            _playerControls.Combat.Dash.performed += _ => Dash();
            _startingMoveSpeed = moveSpeed;
        }

        protected override void Awake() {
            base.Awake();

            _playerControls = new PlayerControls();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _knockback = GetComponent<Knockback>();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void Update()
        {
            PlayerInput();
        }

        private void FixedUpdate()
        {
            AdjustPlayerFacingDirection();
            Move();
        }

        public Transform GetWeaponCollider()
        {
            return weaponCollider;
        }

        private void PlayerInput()
        {
            _movement = _playerControls.Movement.Move.ReadValue<Vector2>();
        
            _animator.SetFloat(MoveX, _movement.x);
            _animator.SetFloat(MoveY, _movement.y);
        }

        private void Move()
        {
            if (_knockback.GettingKnockedBack) return;
            _rigidbody2D.MovePosition(_rigidbody2D.position + _movement * (moveSpeed * Time.fixedDeltaTime));    
        }

        private void AdjustPlayerFacingDirection()
        {
            var playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            var mousePos = Input.mousePosition;

            if (mousePos.x < playerScreenPoint.x)
            {
                _spriteRenderer.flipX = true;
                FacingLeft = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
                FacingLeft = false;
            }
        }

        private void Dash()
        {
            if (!_isDashing)
            {        
                _isDashing = true;
                moveSpeed *= dashSpeed;
                trailRenderer.emitting = true;
                StartCoroutine(EndDashRoutine());
            }

        }

        private IEnumerator EndDashRoutine()
        {
            float dashTime = .2f;
            float dashCd = .25f;
            yield return new WaitForSeconds(dashTime);
            moveSpeed = _startingMoveSpeed;
            trailRenderer.emitting = false;
            yield return new WaitForSeconds(dashCd);
            _isDashing = false;
        }
    }
}
