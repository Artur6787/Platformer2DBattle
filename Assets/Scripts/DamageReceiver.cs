using UnityEngine;

[RequireComponent(typeof(Health))]
public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private int _collisionDamage = 10;

    private Health _health;
    private Invincibility _invincibility;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _invincibility = GetComponent<Invincibility>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (TryGetComponent<Player>(out _) && collision.gameObject.TryGetComponent<Enemy>(out _))
        {
            TakeDamage(_collisionDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (_invincibility != null && _invincibility.IsProtected())
            return;

        _health.TakeDamage(damage);

        if (_invincibility != null)
            _invincibility.MakeProtected();
    }
}