using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed;
    [SerializeField] private float _chaseDistance = 5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _stunDuration = 2f;
    [SerializeField] private Transform _healthBarPivot;

    private Transform[] _points;
    private int _currentPointIndex;
    private Transform _target;
    private bool _isStunned = false;

    private void Start()
    {
        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
        {
            _points[i] = _path.GetChild(i);
        }

        _target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (IsPlayerInSight())
        {
            ChasePlayer();
        }
        else
        {
            MoveAlongPath();
        }

        UpdateHealthBarRotation();
    }

    private void UpdateHealthBarRotation()
    {
        if (_healthBarPivot != null)
        {
            _healthBarPivot.rotation = Quaternion.identity;
        }
    }

    private bool IsPlayerInSight()
    {
        Vector2 directionToPlayer = new Vector2(_target.position.x - transform.position.x, 0).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, _target.position);

        if (distanceToPlayer <= _chaseDistance)
        {
            float dotProduct = Vector2.Dot(transform.right, directionToPlayer);

            if (dotProduct > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, _playerLayer);
                Debug.DrawLine(transform.position, transform.position + (Vector3)directionToPlayer * distanceToPlayer, Color.red);
                return hit.collider != null && hit.collider.CompareTag("Player");
            }
        }

        return false;
    }

    private void ChasePlayer()
    {
        Vector3 directionToPlayer = (_target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        Reflect(directionToPlayer);
    }

    private void MoveAlongPath()
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

        Reflect((target.position - transform.position).normalized);
    }

    public void TakeDamage(int damage)
    {
        HealthEnemy healthEnemy = GetComponent<HealthEnemy>();
        if (healthEnemy != null)
        {
            healthEnemy.ChangePoints(-damage);
        }
    }

    private void Reflect(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

        if (direction.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            if (_healthBarPivot != null)
                _healthBarPivot.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
            if (_healthBarPivot != null)
                _healthBarPivot.localScale = new Vector3(1, 1, 1);
        }
    }
}

//using UnityEngine;

//public class Enemy : MonoBehaviour
//{
//    [SerializeField] private Transform _path; // ���� �������� �����
//    [SerializeField] private float _speed; // �������� �����
//    [SerializeField] private float _chaseDistance = 5f; // ����������, �� ������� ���� �������� ������������ ������
//    [SerializeField] private LayerMask _playerLayer; // ����, �� ������� ��������� �����

//    private Transform[] _points; // ����� ����
//    private int _currentPointIndex; // ������ ������� ����� ����
//    private Transform _target; // ������ �� ������ (Mover)
//    private Health _health; // ������ �� ��������� ��������

//    private void Start()
//    {
//        _points = new Transform[_path.childCount]; // ������������� ������� ����� ����
//        for (int i = 0; i < _path.childCount; i++)
//        {
//            _points[i] = _path.GetChild(i); // ���������� ������� ������� ����
//        }

//        _target = GameObject.FindWithTag("Player").transform; // �������� ������ �� ������ �� ���� "Player"
//        _health = GetComponent<Health>(); // �������� ��������� ��������
//    }

//    public void TakeDamage(int damage)
//    {
//        _health.TakeDamage(damage); // �������� ���� � ��������� ��������
//        // ����� �������� �������������� ������ (��������, ��������������� �������� ��������� �����)
//    }

//    private void Update()
//    {
//        if (IsPlayerInSight())
//        {
//            ChasePlayer(); // ���������� ������, ���� �� � ���� ������
//        }
//        else
//        {
//            MoveAlongPath(); // ��������� �� ����, ���� ����� ��� ���� ������
//        }
//    }

//    private bool IsPlayerInSight()
//    {
//        Vector2 directionToPlayer = new Vector2(_target.position.x - transform.position.x, 0).normalized;
//        float distanceToPlayer = Vector2.Distance(transform.position, _target.position);

//        if (distanceToPlayer <= _chaseDistance)
//        {
//            float dotProduct = Vector2.Dot(transform.right, directionToPlayer);

//            if (dotProduct > 0)
//            {
//                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, _playerLayer);

//                Debug.DrawLine(transform.position, transform.position + (Vector3)directionToPlayer * distanceToPlayer, Color.red); // ������������ ����

//                return hit.collider != null && hit.collider.CompareTag("Player"); // ���������, ������ �� � ������
//            }
//        }

//        return false;
//    }

//    private void ChasePlayer()
//    {
//        Vector3 directionToPlayer = (_target.position - transform.position).normalized;

//        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime); // ��������� � ������

//        Reflect(directionToPlayer); // ������������ ����� � ������� ��������
//    }

//    private void MoveAlongPath()
//    {
//        Transform target = _points[_currentPointIndex];

//        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime); // ��������� � ������� ����� ����

//        if (Vector3.Distance(transform.position, target.position) < 0.1f)
//        {
//            _currentPointIndex++;

//            if (_currentPointIndex >= _points.Length)
//            {
//                _currentPointIndex = 0; // ������������ � ������ ����
//            }
//        }

//        Reflect((target.position - transform.position).normalized); // ������������ ����� � ������� ������� ����� ����
//    }

//    private void Reflect(Vector3 direction)
//    {
//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

//        if (direction.x > 0)
//            GetComponent<SpriteRenderer>().flipX = false; // ����� ������
//        else
//            GetComponent<SpriteRenderer>().flipX = true; // ����� �����
//    }
//}