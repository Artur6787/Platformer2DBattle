using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;

    private int _currentHealth;
    private Collider2D _collider;

    public event Action<float> HealthChanged;
    public event Action Died;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning("ѕопытка нанести отрицательный урон!");
            return;
        }

        ModifyHealth(-Mathf.Abs(damage));
    }

    public void Heal(int healAmount)
    {
        if (healAmount < 0)
        {
            Debug.LogWarning("ѕопытка вылечить отрицательное количество здоровь€!");
            return;
        }

            ModifyHealth(Mathf.Abs(healAmount));
    }

    private void ModifyHealth(int value)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + value, 0, _maxHealth);
        HealthChanged?.Invoke((float)_currentHealth / _maxHealth);

        if (_currentHealth <= 0)
            HandleDeath();
    }

    private void HandleDeath()
    {
        if (_collider != null)
            _collider.enabled = false;

        Died?.Invoke();
    }
}