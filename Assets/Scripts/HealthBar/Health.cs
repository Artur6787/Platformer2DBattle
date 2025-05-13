using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    private int _currentHealth;

    public event Action<float> HealthChanged;
    public event Action Died;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        ModifyHealth(-Mathf.Abs(damage));
    }

    public void Heal(int healAmount)
    {
        ModifyHealth(Mathf.Abs(healAmount));
    }

    private void ModifyHealth(int value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);
        HealthChanged?.Invoke((float)_currentHealth / _maxHealth);

        if (_currentHealth <= 0)
            Died?.Invoke();
    }
}