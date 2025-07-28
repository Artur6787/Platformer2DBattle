using UnityEngine;

public class DirectionHandler : MonoBehaviour
{
    public void Reflect(Vector3 direction)
    {
        if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}