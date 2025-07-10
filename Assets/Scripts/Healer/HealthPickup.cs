using UnityEngine;

public class HealthPickup : CollectibleItem
{
    [SerializeField] private int _healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var player))
        {
            if (player.TryGetComponent<Health>(out var health))
            {
                health.Heal(_healAmount);
                HandleCollected();
            }
        }
    }

    public override void HandleCollected()
    {
        base.HandleCollected();
        Destroy(gameObject);
    }
}