using UnityEngine;

[RequireComponent(typeof(Vision))]
public class Chaser : MonoBehaviour
{
    private Vision vision;

    private void Awake()
    {
        vision = GetComponent<Vision>();
    }

    public bool HasTarget()
    {
        return vision != null && vision.IsPlayerVisible();
    }

    public Vector2 GetTargetPosition()
    {
        return vision != null ? vision.GetTargetPosition() : (Vector2)transform.position;
    }
}