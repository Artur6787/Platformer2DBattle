using UnityEngine;

[RequireComponent(typeof(Chaser))]
[RequireComponent(typeof(Patroller))]
public class Enemy : MonoBehaviour
{
    private Chaser _chaser;
    private Patroller _patroller;

    private void Start()
    {
        _chaser = GetComponent<Chaser>();
        _patroller = GetComponent<Patroller>();
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
}