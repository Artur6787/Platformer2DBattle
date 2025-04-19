using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player playerComponent = collision.GetComponent<Player>();

        if (playerComponent != null)
        {
            Health playerHealth = collision.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.ChangePoints(_healAmount);
                Destroy(gameObject);
            }
        }
    }
}
