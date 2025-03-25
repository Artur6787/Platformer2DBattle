using System;
using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour
{
    public event Action<CollectibleItem> Collected;
    public event Action<CollectibleItem> Destroyed;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Mover movement))
        {
            HandleCollected();
            Destroy(gameObject);
        }
    }

    public void HandleCollected()
    {
        Collected?.Invoke(this);
        Destroyed?.Invoke(this);
    }
}