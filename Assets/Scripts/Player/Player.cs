using UnityEngine;

[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Died += OnDeath;
    }

    private void OnDisable()
    {
        _health.Died -= OnDeath;
    }

    private void OnDeath()
    {
        Debug.Log("Игрок погиб!");
    }
}