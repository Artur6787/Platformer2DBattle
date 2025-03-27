using UnityEngine;

public class Attacker : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        HealthEnemy healthEnemy = GetComponent<HealthEnemy>();

        if (healthEnemy != null)
        {
            healthEnemy.ChangePoints(-damage);
        }
    }
}