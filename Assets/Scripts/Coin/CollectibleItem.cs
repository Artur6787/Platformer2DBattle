using System;
using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour
{
    public event Action<CollectibleItem> Collected;
    public event Action<CollectibleItem> Destroyed;

    public void HandleCollected()
    {
        Collected?.Invoke(this);
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }
}