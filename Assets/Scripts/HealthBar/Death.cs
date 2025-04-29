using UnityEngine;

[RequireComponent(typeof(Health))]
public class Death : MonoBehaviour
{
    private Collider2D _collider;

    private void Awake()
    {
        GetComponent<Health>().Died += Die;
        _collider = GetComponent<Collider2D>();
    }

    private void Die()
    {
        _collider.enabled = false;
        Destroy(gameObject, 0.1f);
    }
}