using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AnimationHandler))]
[RequireComponent(typeof(InputHandler))]
public class AttackController : MonoBehaviour
{
    [SerializeField] private GameObject _attackHitbox;
    [SerializeField] private Vector2 _initialLocalPosition;

    public float startTime;
    public Transform hitPosition;
    public LayerMask enemy;
    public float hitRange;
    public int damageAmount;

    private float _cooldownTime;
    private bool _isHitting = false;
    private SpriteRenderer _spriteRenderer;
    private AnimationHandler _animationHandler;
    private InputHandler _inputHandler;
    private bool _hasHit;

    private void Awake()
    {
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animationHandler = GetComponent<AnimationHandler>();
        _initialLocalPosition = _attackHitbox.transform.localPosition;
    }

    private void OnEnable()
    {
        _inputHandler.OnActionCommand += HandleHitInput;
    }

    private void OnDisable()
    {
        _inputHandler.OnActionCommand -= HandleHitInput;
    }

    private void LateUpdate()
    {
        if (_spriteRenderer.flipX)
        {
            _attackHitbox.transform.localPosition = new Vector3(-_initialLocalPosition.x, _initialLocalPosition.y);
        }
        else
        {
            _attackHitbox.transform.localPosition = _initialLocalPosition;
        }
    }

    private void Update()
    {
        if (_cooldownTime > 0)
        {
            _cooldownTime -= Time.deltaTime;
        }
        else if (_isHitting)
        {
            ResetAttack();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition.position, hitRange);
    }

    public void OnAttackAnimationHit()
    {
        if (_isHitting && _hasHit == false)
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
        if (_isHitting && _hasHit == false)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hitPosition.position, hitRange, enemy);

            foreach (var enemyCollider in enemies)
            {
                Attacker attackerComponent = enemyCollider.GetComponent<Attacker>();

                if (attackerComponent != null)
                {
                    attackerComponent.TakeDamage(damageAmount);
                }
            }

            _hasHit = true;
        }
    }

    private void StartAttack()
    {
        _isHitting = true;
        _animationHandler.SetAttackState(true);
        _cooldownTime = startTime;
        _hasHit = false;
    }

    private void HandleHitInput()
    {
        if (_cooldownTime <= 0 && _isHitting == false)
        {
            StartAttack();
        }
    }

    private void ResetAttack()
    {
        _isHitting = false;
        _animationHandler.SetAttackState(false);
        _hasHit = false;
    }
}