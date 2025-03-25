using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{
    [SerializeField] private Image _barFilling;
    [SerializeField] private HealthEnemy _pointsSource;

    private void Awake()
    {
        _pointsSource.PointsChanged += OnPointsUpdated;
    }

    private void OnDestroy()
    {
        _pointsSource.PointsChanged -= OnPointsUpdated;
    }

    private void OnPointsUpdated(float valueAsPecantage)
    {
        _barFilling.fillAmount = valueAsPecantage;
    }
}