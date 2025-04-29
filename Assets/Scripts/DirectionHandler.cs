using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DirectionHandler : MonoBehaviour
{
    [SerializeField] private Transform _healthBarPivot;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
            _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Reflect(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));

        if (direction.x > 0)
        {
            _spriteRenderer.flipX = false;
            _healthBarPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            _spriteRenderer.flipX = false;
            _healthBarPivot.transform.rotation = Quaternion.Euler(-1, 0, 0);
        }
    }
}