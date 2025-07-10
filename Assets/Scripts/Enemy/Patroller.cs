using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Patroller : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _speed;
    [SerializeField] private float _pointReachThreshold = 0.1f;

    private int _currentPointIndex;
    private DirectionHandler _directionHandler;

    private void Start()
    {
        _directionHandler = GetComponent<DirectionHandler>();
    }

    public void MoveAlongPath()
    {
        if (_directionHandler == null)
            return;

        Transform target = _points[_currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);

        if ((transform.position - target.position).sqrMagnitude < _pointReachThreshold * _pointReachThreshold)
        {
            _currentPointIndex = (_currentPointIndex + 1) % _points.Length;
        }

        _directionHandler.Reflect((target.position - transform.position).normalized);
    }
}