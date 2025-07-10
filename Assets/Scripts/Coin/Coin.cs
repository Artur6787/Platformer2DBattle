using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Coin : CollectibleItem
{
    [HideInInspector] public SpawnPoint OriginPoint;
}