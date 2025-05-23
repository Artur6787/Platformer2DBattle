using UnityEngine;

[RequireComponent(typeof(DirectionHandler))]
public class Patroller : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;

    private Transform[] _points;
    private int _currentPointIndex;
    private DirectionHandler _directionHandler;

    private void Start()
    {
        _directionHandler = GetComponent<DirectionHandler>();
        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i);
        }
    }

    public void MoveAlongPath()
    {
        Transform target = _points[_currentPointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            _currentPointIndex++;

            if (_currentPointIndex >= _points.Length)
            {
                _currentPointIndex = 0;
            }
        }

        _directionHandler.Reflect((target.position - transform.position).normalized);
    }
}