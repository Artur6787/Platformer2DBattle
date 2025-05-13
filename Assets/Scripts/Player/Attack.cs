using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AnimationHandler))]
[RequireComponent(typeof(InputHandler))]
public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject _attackHitbox;
    [SerializeField] private Vector2 _initialLocalPosition;
    [SerializeField] private float _startTime = 0.1f;
    [SerializeField] private Transform _hitPosition;
    [SerializeField] private LayerMask _enemy;
    [SerializeField] private float _hitRange;
    [SerializeField] private int _damageAmount;

    private bool _isHitting;
    private SpriteRenderer _spriteRenderer;
    private AnimationHandler _animationHandler;
    private InputHandler _inputHandler;
    private bool _hasHit;
    private Mover _mover;
    private float _attackTimeRemaining;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
        _mover = GetComponent<Mover>();
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animationHandler = GetComponent<AnimationHandler>();
        _initialLocalPosition = _attackHitbox.transform.localPosition;
    }

    private void OnEnable()
    {
        _inputHandler.ActionCommand += HandleHitInput;
    }

    private void OnDisable()
    {
        _inputHandler.ActionCommand -= HandleHitInput;
    }

    private void LateUpdate()
    {
        _attackHitbox.transform.localPosition = _spriteRenderer.flipX
            ? new Vector3(-_initialLocalPosition.x, _initialLocalPosition.y)
            : _initialLocalPosition;
    }

    private void Update()
    {       
        if (_isHitting)
        {
            _attackTimeRemaining -= Time.deltaTime;

            if (_attackTimeRemaining <= 0)
            {
                ResetAttack();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_hitPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_hitPosition.position, _hitRange);
        }
    }

    public void OnAttackAnimationHit()
    {
        if (_isHitting == true && _hasHit == false)
        {
            PerformHit();
        }
    }

    public void OnAttackAnimationEnd()
    {
        ResetAttack();
    }

    private void PerformHit()
    {
        if (_isHitting == true && _hasHit == false)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(_hitPosition.position, _hitRange, _enemy);

            foreach (var enemyCollider in enemies)
            {
                if (enemyCollider.TryGetComponent(out Health health))
                {
                    health.TakeDamage(_damageAmount);
                }
            }

            _hasHit = true;
        }
    }

    private void StartAttack()
    {
        _isHitting = true;
        _animationHandler.SetAttackState(true);
        _hasHit = false;
        _attackTimeRemaining = _startTime;

        if (_mover != null)
        {
            _mover.SetAttackingState(true);
        }
    }

    private void HandleHitInput()
    {
        if (_isHitting == false)
        {
            StartAttack();
        }
    }

    private void ResetAttack()
    {
        _isHitting = false;
        _animationHandler.SetAttackState(false);
        _hasHit = false;

        if (_mover != null)
        {
            _mover.SetAttackingState(false);
        }
    }
}