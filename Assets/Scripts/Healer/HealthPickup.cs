using UnityEngine;

public class HealthPickup : CollectibleItem
{
    [SerializeField] private int _healAmount = 20;

    public int HealAmount
    {
        get
        {
            return _healAmount;
        }
    }

    public override void HandleCollected()
    {
        base.HandleCollected();
        Destroy(gameObject);
    }
}