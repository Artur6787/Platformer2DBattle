using UnityEngine;

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
    private bool _hasHit = false;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animationHandler = GetComponent<AnimationHandler>();
        _inputHandler = GetComponent<InputHandler>();
        _initialLocalPosition = _attackHitbox.transform.localPosition;
        _inputHandler.OnActionCommand += HandleHitInput;
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
        if (_cooldownTime <= 0)
        {
            if (_isHitting)
            {
                _isHitting = false;
                _animationHandler.SetAttackState(false);
                _hasHit = false;
            }
        }
        else
        {
            _cooldownTime -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        _inputHandler.OnActionCommand -= HandleHitInput;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition.position, hitRange);
    }

    public void PerformHit()
    {
        if (_hasHit == false)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hitPosition.position, hitRange, enemy);

            for (int i = 0; i < enemies.Length; i++)
            {
                Attacker attackerComponent = enemies[i].GetComponent<Attacker>();

                if (attackerComponent != null)
                {
                    attackerComponent.TakeDamage(damageAmount);
                }
            }

            _hasHit = true;
        }
    }

    private void HandleHitInput()
    {
        if (_cooldownTime <= 0 && _isHitting == false)
        {
            _isHitting = true;
            _animationHandler.SetAttackState(true);
            _hasHit = false;
            PerformHit();
            _hasHit = true;
            _cooldownTime = startTime;
        }
    }
}