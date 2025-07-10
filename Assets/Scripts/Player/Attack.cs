using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AnimationHandler))]
public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject _attackHitbox;
    [SerializeField] private Vector2 _initialLocalPosition;
    [SerializeField] private float _startTime = 0.2f;
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
        _inputHandler = GetComponentInParent<InputHandler>();
        _mover = GetComponentInParent<Mover>();
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
        float directionSign = Mathf.Sign(transform.localScale.x);
        _attackHitbox.transform.localPosition = new Vector3(
        _initialLocalPosition.x * directionSign, _initialLocalPosition.y, 0);
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
        Debug.Log("PerformHit вызван");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(_hitPosition.position, _hitRange, _enemy);
        Debug.Log("Найдено врагов: " + enemies.Length);

        foreach (var enemyCollider in enemies)
        {
            Debug.Log("Пробуем нанести урон: " + enemyCollider.name);

            if (enemyCollider.TryGetComponent<DamageReceiver>(out var damageReceiver))
            {
                Debug.Log("DamageReceiver найден, наносим урон: " + enemyCollider.name);
                damageReceiver.TakeDamage(_damageAmount);
            }
            else
            {
                Debug.Log("DamageReceiver НЕ найден у: " + enemyCollider.name);
            }
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