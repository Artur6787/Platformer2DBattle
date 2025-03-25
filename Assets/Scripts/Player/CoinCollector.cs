using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Mover mover))
        {
            if (collision.gameObject.TryGetComponent(out CollectibleItem collectible))
            {
                collectible.HandleCollected();
            }
        }
    }
}