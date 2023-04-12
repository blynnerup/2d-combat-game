using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft
    {
        get => _facingLeft;
        set => _facingLeft = value;
    }

    public static PlayerController Instance;
    
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer trailRenderer;

    private PlayerControls _playerControls;
    private Vector2 _movement;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveY = Animator.StringToHash("moveY");
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private float _startingMoveSpeed;

    private bool _facingLeft = false;
    private bool _isDashing = false;

    private void Start()
    {
        _camera = Camera.main;
        _playerControls.Combat.Dash.performed += _ => Dash();
        _startingMoveSpeed = moveSpeed;
    }

    private void Awake()
    {
        Instance = this;
        _playerControls = new PlayerControls();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
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

    private void PlayerInput()
    {
        _movement = _playerControls.Movement.Move.ReadValue<Vector2>();
        
        _animator.SetFloat(MoveX, _movement.x);
        _animator.SetFloat(MoveY, _movement.y);
    }

    private void Move()
    {
        _rigidbody2D.MovePosition(_rigidbody2D.position + _movement * (moveSpeed * Time.fixedDeltaTime));    
    }

    private void AdjustPlayerFacingDirection()
    {
        var playerScreenPoint = _camera.WorldToScreenPoint(transform.position);
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
