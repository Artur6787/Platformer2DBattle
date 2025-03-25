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
//    [SerializeField] private Transform _path; // Путь движения врага
//    [SerializeField] private float _speed; // Скорость врага
//    [SerializeField] private float _chaseDistance = 5f; // Расстояние, на котором враг начинает преследовать игрока
//    [SerializeField] private LayerMask _playerLayer; // Слой, на котором находится игрок

//    private Transform[] _points; // Точки пути
//    private int _currentPointIndex; // Индекс текущей точки пути
//    private Transform _target; // Ссылка на игрока (Mover)
//    private Health _health; // Ссылка на компонент здоровья

//    private void Start()
//    {
//        _points = new Transform[_path.childCount]; // Инициализация массива точек пути
//        for (int i = 0; i < _path.childCount; i++)
//        {
//            _points[i] = _path.GetChild(i); // Заполнение массива точками пути
//        }

//        _target = GameObject.FindWithTag("Player").transform; // Получаем ссылку на игрока по тегу "Player"
//        _health = GetComponent<Health>(); // Получаем компонент здоровья
//    }

//    public void TakeDamage(int damage)
//    {
//        _health.TakeDamage(damage); // Передаем урон в компонент здоровья
//        // Можно добавить дополнительную логику (например, воспроизведение анимации получения урона)
//    }

//    private void Update()
//    {
//        if (IsPlayerInSight())
//        {
//            ChasePlayer(); // Преследуем игрока, если он в поле зрения
//        }
//        else
//        {
//            MoveAlongPath(); // Двигаемся по пути, если игрок вне поля зрения
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

//                Debug.DrawLine(transform.position, transform.position + (Vector3)directionToPlayer * distanceToPlayer, Color.red); // Визуализация луча

//                return hit.collider != null && hit.collider.CompareTag("Player"); // Проверяем, попали ли в игрока
//            }
//        }

//        return false;
//    }

//    private void ChasePlayer()
//    {
//        Vector3 directionToPlayer = (_target.position - transform.position).normalized;

//        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime); // Двигаемся к игроку

//        Reflect(directionToPlayer); // Поворачиваем врага в сторону движения
//    }

//    private void MoveAlongPath()
//    {
//        Transform target = _points[_currentPointIndex];

//        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime); // Двигаемся к текущей точке пути

//        if (Vector3.Distance(transform.position, target.position) < 0.1f)
//        {
//            _currentPointIndex++;

//            if (_currentPointIndex >= _points.Length)
//            {
//                _currentPointIndex = 0; // Возвращаемся к началу пути
//            }
//        }

//        Reflect((target.position - transform.position).normalized); // Поворачиваем врага в сторону текущей точки пути
//    }

//    private void Reflect(Vector3 direction)
//    {
//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

//        if (direction.x > 0)
//            GetComponent<SpriteRenderer>().flipX = false; // Лицом вправо
//        else
//            GetComponent<SpriteRenderer>().flipX = true; // Лицом влево
//    }
//}