using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<CollectibleItem>(out var collectible))
        {
            collectible.HandleCollected();
        }
    }
}