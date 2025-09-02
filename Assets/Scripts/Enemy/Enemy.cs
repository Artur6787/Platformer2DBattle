using UnityEngine;

[RequireComponent(typeof(Chaser))]
[RequireComponent(typeof(Patroller))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyMover))]
[RequireComponent(typeof(DirectionHandler))]
[RequireComponent(typeof(Vision))]
public class Enemy : MonoBehaviour
{
    private EnemyMover _mover;
    private Chaser _chaser;
    private Patroller _patroller;
    private Health _health;
    private DirectionHandler _directionHandler;
    private Vision _vision;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _chaser = GetComponent<Chaser>();
        _patroller = GetComponent<Patroller>();
        _health = GetComponent<Health>();
        _directionHandler = GetComponent<DirectionHandler>();
        _vision = GetComponent<Vision>();
    }

    private void FixedUpdate()
    {
        Vector2 targetPos;

        if (_vision != null && _vision.IsPlayerVisible())
        {
            targetPos = _vision.GetTargetPosition();
        }
        else
        {
            targetPos = _patroller.GetNextTargetPosition();
        }

        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        _mover.MoveTowards(targetPos);
        _directionHandler.Reflect(direction);
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