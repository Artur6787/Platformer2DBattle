using UnityEngine;
using UnityEngine.UI;

public class HealthBarForAll : MonoBehaviour
{
    [SerializeField] private Image _barFilling;
    [SerializeField] private Health _pointsSource;

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