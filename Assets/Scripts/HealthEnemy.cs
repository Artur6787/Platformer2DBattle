using System;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    [SerializeField] private int _maxPoints = 100;
    [SerializeField] private int _damageFromPlayer = 10;

    public event Action<float> PointsChanged;

    private int _currentPoints;
    private Rigidbody2D _rb;    

    private void Start()
    {
        _currentPoints = _maxPoints;
        _rb = GetComponent<Rigidbody2D>();
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
        GetComponent<Collider2D>().enabled = false; // Отключаем коллайдер
        Destroy(gameObject, 0.1f); // Небольшая задержка для завершения анимации
    }
}