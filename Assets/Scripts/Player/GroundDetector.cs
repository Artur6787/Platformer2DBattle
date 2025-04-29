using System;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private float checkDistance = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 offset = Vector2.zero;

    public event Action<bool> OnGroundedChanged;

    public bool IsGrounded { get; private set; }

    private void FixedUpdate()
    {
        Vector2 origin = (Vector2)transform.position + offset;
        bool wasGrounded = IsGrounded;
        IsGrounded = Physics2D.Raycast(origin, Vector2.down, checkDistance, groundLayer);

        if (IsGrounded != wasGrounded)
        {
            OnGroundedChanged?.Invoke(IsGrounded);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 origin = (Vector2)transform.position + offset;
        Gizmos.DrawLine(origin, origin + Vector2.down * checkDistance);
    }
}