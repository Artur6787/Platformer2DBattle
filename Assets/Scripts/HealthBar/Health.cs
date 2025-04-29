using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private int _maxPoints = 100;

    private int _currentPoints;

    public event Action<float> PointsChanged;
    public event Action Died;

    private void Start()
    {
        _currentPoints = _maxPoints;
    }

    public void ChangePoints(int value)
    {
        _currentPoints = Mathf.Clamp(_currentPoints + value, 0, _maxPoints);
        float currentHealthAsPercentage = (float)_currentPoints / _maxPoints;

        PointsChanged?.Invoke(currentHealthAsPercentage);

        if (_currentPoints <= 0)
        {
            Died?.Invoke();
        }
    }
}