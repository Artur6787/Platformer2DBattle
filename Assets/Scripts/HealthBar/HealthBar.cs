using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _barFilling;
    [SerializeField] private Health _pointsSource;

    private void Awake()
    {
        _pointsSource.HealthChanged += OnPointsUpdated;
    }

    private void OnDestroy()
    {
        _pointsSource.HealthChanged -= OnPointsUpdated;
    }

    private void OnPointsUpdated(float valueAsPecantage)
    {
        _barFilling.fillAmount = valueAsPecantage;
    }
}