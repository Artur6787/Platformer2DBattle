using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private int _healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthPlayer playerHealth = collision.GetComponent<HealthPlayer>();
        if (playerHealth != null)
        {
            playerHealth.ChangePoints(_healAmount);
            Destroy(gameObject);
        }
    }
}
