using UnityEngine;

[RequireComponent(typeof(Chaser))]
[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    private Chaser _chaser;
    private Patroller _patroller;
    private Health _health;

    private void Awake()
    {
        _chaser = GetComponent<Chaser>();
        _patroller = GetComponent<Patroller>();
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        if (_chaser.IsPlayerInSight())
        {
            _chaser.ChasePlayer();
        }
        else
        {
            _patroller.MoveAlongPath();
        }
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
        Debug.Log($"{name} уничтожен");
    }
}