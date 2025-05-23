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

    public event Action<bool> GroundedStateChanged;
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

        if (_directionHandler != null && _currentInputVector.x != 0 && _isAttacking == false)
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
        _inputHandler.MoveCommand += HandleMovementInput;
        _inputHandler.JumpCommand += HandleJumpInput;
        _inputHandler.ActionCommand += HandleActionRequest;
        _groundDetector.GroundedChanged += SetGroundedState;
    }

    private void OnDisable()
    {
        _inputHandler.MoveCommand -= HandleMovementInput;
        _inputHandler.JumpCommand -= HandleJumpInput;
        _inputHandler.ActionCommand -= HandleActionRequest;
        _groundDetector.GroundedChanged -= SetGroundedState;
    }

    private void HandleActionRequest()
    {
        if (_isAttacking == false)
        {
            SetAttackingState(true);
        }
    }

    public void SetAttackingState(bool isAttacking)
    {
        _isAttacking = isAttacking;
        _animationHandler.SetAttackState(_isAttacking);
        
        if (isAttacking)
        {
            AttackStarted?.Invoke();
        }
        else
        {
            AttackEnded?.Invoke();
        }
    }

    public void ResetAttack()
    {
        SetAttackingState(false);
    }

    private void HandleMovementInput(Vector2 moveInput)
    {
        _currentInputVector = moveInput;
    }

    private void HandleJumpInput()
    {
        if (IsGrounded && _isAttacking == false)
        {
            _jumpRequested = true;
        }
    }

    private void ProcessMovement()
    {
        if (_isAttacking == false)
        {
            _rigidbody2d.velocity = new Vector2(_currentInputVector.x * _speed, _rigidbody2d.velocity.y);
        }
        else
        {
            _rigidbody2d.velocity = new Vector2(0, _rigidbody2d.velocity.y);
        }
    }

    private void Jump()
    {
        if (_jumpRequested && _isAttacking == false)
        {
            _rigidbody2d.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            SetGroundedState(false);
            _jumpRequested = false;
        }
    }

    private void UpdateAnimation()
    {
        bool isJumping = IsGrounded == false;
        bool isRunning = _currentInputVector.x != 0 && IsGrounded && _isAttacking == false;
        _animationHandler.UpdateJumpState(isJumping);
        _animationHandler.UpdateRunState(isRunning);
    }

    private void SetGroundedState(bool grounded)
    {
        if (IsGrounded != grounded)
        {
            IsGrounded = grounded;
            GroundedStateChanged?.Invoke(IsGrounded);
        }
    }
}