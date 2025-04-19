using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxPoints = 100;

    public event Action<float> PointsChanged;

    private int currentPoints;
    private Rigidbody2D _rigidbody;
    private new Collider2D collider;

    private void Start()
    {
        currentPoints = _maxPoints;
        _rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    public void ChangePoints(int value)
    {
        currentPoints = Mathf.Clamp(currentPoints + value, 0, _maxPoints);
        float currentHealthAsPercentage = (float)currentPoints / _maxPoints;

        if (currentPoints <= 0)
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
        collider.enabled = false;
        Destroy(gameObject, 0.1f);
    }
}