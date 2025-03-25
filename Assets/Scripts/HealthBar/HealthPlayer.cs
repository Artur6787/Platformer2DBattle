using System;
using UnityEngine;

public class HealthPlayer : MonoBehaviour
{
    [SerializeField] private int _maxPoints = 100;
    [SerializeField] private int _damageFromEnemy = 10;

    public event Action<float> PointsChanged;

    private int _currentPoints;
    private Invincibility _invincibility;

    private void Start()
    {
        _currentPoints = _maxPoints;
        _invincibility = GetComponent<Invincibility>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            if (_invincibility.IsProtected() == false)
            {
                ChangePoints(-_damageFromEnemy);
                _invincibility.MakeProtected();
            }
        }
    }

    public void ChangePoints(int value)
    {
        _currentPoints = Mathf.Clamp(_currentPoints + value, 0, _maxPoints);
        float currentHealthAsPercentage = (float)_currentPoints / _maxPoints;

        if (_currentPoints <= 0)
        {
            Die();
        }
        else
        {
            PointsChanged?.Invoke(currentHealthAsPercentage);
        }
    }

    private void Die()
    {
        PointsChanged?.Invoke(0);
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 0.1f);
    }
}