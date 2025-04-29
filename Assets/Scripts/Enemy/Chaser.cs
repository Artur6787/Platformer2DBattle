using UnityEngine;

[RequireComponent(typeof(DirectionHandler))]
public class Chaser : MonoBehaviour
{
    [SerializeField] private float _chaseDistance = 5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _player;

    private Transform _target;
    private DirectionHandler _directionHandler;

    private void Start()
    {
        _directionHandler = GetComponent<DirectionHandler>();

        if (_player != null)
        {
            _target = _player.transform;
        }
    }

    public bool IsPlayerInSight()
    {
        if (_target == null)
        {
            return false;
        }

        Vector2 directionToPlayer = new Vector2(_target.position.x - transform.position.x, 0).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, _target.position);

        if (distanceToPlayer <= _chaseDistance)
        {
            float dotProduct = Vector2.Dot(transform.right, directionToPlayer);

            if (dotProduct > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, _playerLayer);
                Debug.DrawLine(transform.position, transform.position + (Vector3)directionToPlayer * distanceToPlayer, Color.red);
                return hit.collider != null && hit.collider.GetComponent<Player>();
            }
        }

        return false;
    }

    public void ChasePlayer()
    {
        if (_target == null)
        {
            return;
        }

        Vector3 directionToPlayer = (_target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        _directionHandler.Reflect(directionToPlayer);
    }
}