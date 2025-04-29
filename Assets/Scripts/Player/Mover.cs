using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimationHandler))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(DirectionHandler))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private DirectionHandler _directionHandler;

    private Rigidbody2D _rigidbody2d;
    private AnimationHandler _animationHandler;
    private InputHandler _inputHandler;
    private Vector2 _currentInputVector;
    private GroundDetector _groundDetector;
    private bool _jumpRequested;
    private bool _isAttacking;

    public event Action<bool> OnGroundedStateChanged;
    public event Action AttackStarted;
    public event Action AttackEnded;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animationHandler = GetComponent<AnimationHandler>();
        _inputHandler = GetComponent<InputHandler>();
        _directionHandler = GetComponent<DirectionHandler>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    private void Update()
    {
        UpdateAnimation();

        if (_directionHandler != null && _currentInputVector.x != 0)
        {
            Vector3 direction = new Vector3(_currentInputVector.x, 0, 0);
            _directionHandler.Reflect(direction);
        }
    }

    private void FixedUpdate()
    {
        ProcessMovement();
        Jump();
    }

    private void OnEnable()
    {
        _inputHandler.OnMoveCommand += HandleMovementInput;
        _inputHandler.OnJumpCommand += HandleJumpInput;
        _inputHandler.OnActionCommand += HandleActionRequest;
        _groundDetector.OnGroundedChanged += SetGroundedState;
    }

    private void OnDisable()
    {
        _inputHandler.OnMoveCommand -= HandleMovementInput;
        _inputHandler.OnJumpCommand -= HandleJumpInput;
        _inputHandler.OnActionCommand -= HandleActionRequest;
        _groundDetector.OnGroundedChanged -= SetGroundedState;
    }

    private void HandleActionRequest()
    {
        if (_isAttacking == false)
        {
            _isAttacking = true;
            _animationHandler.SetAttackState(true);
            AttackStarted?.Invoke();
        }
    }

    private void ResetAttack()
    {
        _isAttacking = false;
        _animationHandler.SetAttackState(false);
        AttackEnded?.Invoke();
    }

    private void HandleMovementInput(Vector2 moveInput)
    {
        _currentInputVector = moveInput;
    }

    private void HandleJumpInput()
    {
        if (IsGrounded)
        {
            _jumpRequested = true;
        }
    }

    private void ProcessMovement()
    {
        _rigidbody2d.velocity = new Vector2(_currentInputVector.x * _speed, _rigidbody2d.velocity.y);
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
        bool isJumping = !IsGrounded;
        bool isRunning = _currentInputVector.x != 0 && IsGrounded;
        _animationHandler.UpdateJumpState(isJumping);
        _animationHandler.UpdateRunState(isRunning);
    }

    private void SetGroundedState(bool grounded)
    {
        if (IsGrounded != grounded)
        {
            IsGrounded = grounded;
            OnGroundedStateChanged?.Invoke(IsGrounded);
        }
    }
}