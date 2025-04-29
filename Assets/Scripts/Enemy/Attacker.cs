using UnityEngine;

[RequireComponent(typeof(Health))]
public class Attacker : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        Health health = GetComponent<Health>();

        if (health != null)
        {
            health.ChangePoints(-damage);
        }
    }
}