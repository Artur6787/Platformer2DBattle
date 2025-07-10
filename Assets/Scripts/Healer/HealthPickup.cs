using UnityEngine;

public class HealthPickup : CollectibleItem
{
    [SerializeField] private int _healAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out var player))
        {
            if (player.TryGetComponent<Health>(out var health))
            {
                health.Heal(_healAmount);
                HandleCollected(); // ���������� ������� �����
            }
        }
    }

    public override void HandleCollected()
    {
        base.HandleCollected(); // ����� ��� ������ �������
        Destroy(gameObject); // ���������� ������ ����� �����
    }
}


//using UnityEngine;

//public class HealthPickup : MonoBehaviour
//{
//    [SerializeField] private int _healAmount = 20;

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        Player playerComponent = collision.GetComponent<Player>();

//        if (playerComponent != null)
//        {
//            Health playerHealth = collision.GetComponent<Health>();

//            if (playerHealth != null)
//            {
//                playerHealth.Heal(_healAmount);
//                Destroy(gameObject);
//            }
//        }
//    }
//}