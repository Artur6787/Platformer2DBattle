using System.Collections;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    [SerializeField] private float protectionDuration = 2f;
    [SerializeField] private float blinkSpeed = 0.1f;

    private bool isProtected = false;
    private Renderer objectRenderer;
    private Collider2D objectCollider;
    private WaitForSeconds blinkWait;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider2D>();
        blinkWait = new WaitForSeconds(blinkSpeed);
    }

    public void MakeProtected()
    {
        if (isProtected == false)
        {
            isProtected = true;
            StartCoroutine(Blinking());
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
            Invoke(nameof(DisableProtection), protectionDuration);
        }
    }

    private IEnumerator Blinking()
    {
        while (isProtected)
        {
            objectRenderer.enabled = false;
            yield return blinkWait;
            objectRenderer.enabled = true;
            yield return blinkWait;
        }
    }

    private void DisableProtection()
    {
        isProtected = false;
        objectRenderer.enabled = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    public bool IsProtected()
    {
        return isProtected;
    }
}