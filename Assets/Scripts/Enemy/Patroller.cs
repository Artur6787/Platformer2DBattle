using UnityEngine;

public class Patroller : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _pointReachThreshold = 0.1f;

    private const int NextPointStep = 1;

    private int _currentPointIndex = 0;
    private float _pointReachThresholdSqr;

    private void Awake()
    {
        _pointReachThresholdSqr = _pointReachThreshold * _pointReachThreshold;
    }

    public Vector2 GetNextTargetPosition()
    {
        if (_points == null || _points.Length == 0)
            return transform.position;

        Transform targetPoint = _points[_currentPointIndex];
        float sqrDistance = ((Vector2)transform.position - (Vector2)targetPoint.position).sqrMagnitude;

        if (sqrDistance < _pointReachThresholdSqr)
        {
            _currentPointIndex += NextPointStep;

            if (_currentPointIndex >= _points.Length)
                _currentPointIndex -= _points.Length;

            targetPoint = _points[_currentPointIndex];
        }

        return targetPoint.position;
    }
}