using UnityEngine;

public class Chaser : MonoBehaviour
{
    [SerializeField] private float _chaseDistance = 5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _speed;

    private Transform _target;
    private DirectionHandler _directionHandler;
    private Invincibility _playerInvincibility;

    private void Start()
    {
        _directionHandler = GetComponent<DirectionHandler>();
        Player _player = FindObjectOfType<Player>();

        if (_player != null)
        {
            _target = _player.transform;
            _playerInvincibility = _player.GetComponent<Invincibility>();
        }
    }

    public bool IsPlayerInSight()
    {
        if (_target == null)
            return false;

        if (_playerInvincibility != null && _playerInvincibility.IsProtected())
            return false;

        float directionSign = Mathf.Sign(_target.position.x - transform.position.x);
        Vector2 directionToPlayer = new Vector2(directionSign, 0);
        float sqrDistanceToPlayer = ((Vector2)_target.position - (Vector2)transform.position).sqrMagnitude;
        float sqrChaseDistance = _chaseDistance * _chaseDistance;

        if (sqrDistanceToPlayer <= sqrChaseDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, Mathf.Sqrt(sqrDistanceToPlayer), _playerLayer);
            Debug.DrawLine(transform.position, transform.position + (Vector3)directionToPlayer * Mathf.Sqrt(sqrDistanceToPlayer), Color.red);
            return hit.collider != null && hit.collider.GetComponent<Player>();
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