using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    public event Action<bool> OnGroundedStateChanged;
    public event Action OnAttackStarted;
    public event Action OnAttackEnded;

    public bool IsGrounded => _isGrounded;
    private bool _isGrounded;
    private Rigidbody2D _rigidbody2d;
    private AnimationHandler _animationHandler;
    private SpriteRenderer _sprite;
    private int _facingDirection;
    private InputHandler _inputHandler;
    private Vector2 _currentInputVector;
    private bool _jumpRequested;
    private bool _isAttacking;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animationHandler = GetComponent<AnimationHandler>();
        _sprite = GetComponent<SpriteRenderer>();
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        Reflect();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        ProcessMovement();
        Jump();
        CheckGroundStatus();
    }

    private void OnEnable()
    {
        _inputHandler.OnMoveCommand += HandleMovementInput;
        _inputHandler.OnJumpCommand += HandleJumpInput;
        _inputHandler.OnActionCommand += HandleActionRequest;
    }

    private void OnDestroy()
    {
        _inputHandler.OnActionCommand -= HandleActionRequest;
    }

    private void HandleActionRequest()
    {
        if (_isAttacking == false)
        {
            _isAttacking = true;
            _animationHandler.SetAttackState(true);
            OnAttackStarted?.Invoke();
        }
    }

    private void ResetAttack()
    {
        _isAttacking = false;
        _animationHandler.SetAttackState(false);
        OnAttackEnded?.Invoke();
    }

    private void HandleMovementInput(Vector2 moveInput)
    {
        _currentInputVector = moveInput;
    }

    private void HandleJumpInput()
    {
        if (_isGrounded)
        {
            _jumpRequested = true;
        }
    }

    private void ProcessMovement()
    {
        _rigidbody2d.velocity = new Vector2(_currentInputVector.x * _speed, _rigidbody2d.velocity.y);
    }

    private void Reflect()
    {
        if (_currentInputVector.x > 0)
        {
            _sprite.flipX = false;
            _facingDirection = 1;
        }
        else if (_currentInputVector.x < 0)
        {
            _sprite.flipX = true;
            _facingDirection = -1;
        }
    }

    private void Jump()
    {
        if (_jumpRequested)
        {
            _rigidbody2d.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            SetGroundedState(false);
            _jumpRequested = false;
        }
    }

    private void UpdateAnimation()
    {
        bool isJumping = !_isGrounded;
        bool isRunning = _currentInputVector.x != 0 && _isGrounded;
        _animationHandler.UpdateJumpState(isJumping);

        if (isJumping == false)
            _animationHandler.UpdateRunState(isRunning);
        else
            _animationHandler.UpdateRunState(false);
    }

    public void SetGroundedState(bool state)
    {
        if (_isGrounded != state)
        {
            _isGrounded = state;
            OnGroundedStateChanged?.Invoke(_isGrounded);
        }
    }

    private void CheckGroundStatus()
    {
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer);

        if (hit.collider != null)
        {
            SetGroundedState(true);
        }
        else
        {
            SetGroundedState(false);
        }
    }
}