using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private Health health;
    private Invincibility invincibility;
    private int damageFromEnemy = 10;

    private void Start()
    {
        health = GetComponent<Health>();
        invincibility = GetComponent<Invincibility>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            if (invincibility.IsProtected() == false)
            {
                health.ChangePoints(-damageFromEnemy);
                invincibility.MakeProtected();
            }
        }
    }
}