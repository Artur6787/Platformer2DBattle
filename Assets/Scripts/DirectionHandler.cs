using UnityEngine;

public class DirectionHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Reflect(Vector3 direction)
    {
        Vector3 scale = transform.localScale;

        if (direction.x > 0)
            scale.x = Mathf.Abs(scale.x);
        else if (direction.x < 0)
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}