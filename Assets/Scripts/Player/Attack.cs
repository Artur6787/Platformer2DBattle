using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameObject _childObject;
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

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animationHandler = GetComponent<AnimationHandler>();
        _inputHandler = GetComponent<InputHandler>();
        _initialLocalPosition = _childObject.transform.localPosition;
        _inputHandler.OnActionCommand += HandleHitInput;
    }

    void LateUpdate()
    {
        if (_spriteRenderer.flipX)
        {
            _childObject.transform.localPosition = new Vector3(-_initialLocalPosition.x, _initialLocalPosition.y/*, _initialLocalPosition.z*/);
        }
        else
        {
            _childObject.transform.localPosition = _initialLocalPosition;
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

    public void PerformHit()
    {
        if (_hasHit == false)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(hitPosition.position, hitRange, enemy);

            for (int i = 0; i < enemies.Length; i++)
            {
                Enemy enemyComponent = enemies[i].GetComponent<Enemy>();

                if (enemyComponent != null)
                {
                    enemyComponent.TakeDamage(damageAmount);
                }
            }

            _hasHit = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitPosition.position, hitRange);
    }
}