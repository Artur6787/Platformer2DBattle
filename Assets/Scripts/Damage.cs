using UnityEngine;

[RequireComponent(typeof(Health))]
public class Damage : MonoBehaviour
{
    [SerializeField] private int _collisionDamage = 10;

    private Health _health;
    private Invincibility _invincibility;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _invincibility = GetComponent<Invincibility>();
    }

    public void TakeDamage(int damage)
    {
        if (_invincibility != null && _invincibility.IsProtected())
            return;

        _health.TakeDamage(damage);

        if (_invincibility != null)
            _invincibility.MakeProtected();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Enemy>() != null)
        {
            TakeDamage(_collisionDamage);
        }
    }
}