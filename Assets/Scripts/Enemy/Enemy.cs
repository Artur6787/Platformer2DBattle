using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _healthBarPivot;

    private Chaser _chaser;
    private Patroller _patroller;
    private Attacker _attacker;

    private void Start()
    {
        _chaser = GetComponent<Chaser>();
        _patroller = GetComponent<Patroller>();
        _attacker = GetComponent<Attacker>();
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

        UpdateHealthBarRotation();
    }

    private void UpdateHealthBarRotation()
    {
        if (_healthBarPivot != null)
        {
            _healthBarPivot.rotation = Quaternion.identity;
        }
    }

    public void Reflect(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

        if (direction.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;

            if (_healthBarPivot != null)
                _healthBarPivot.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;

            if (_healthBarPivot != null)
                _healthBarPivot.localScale = new Vector3(-1, 1, 1);
        }
    }
}