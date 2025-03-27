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
            Invoke(nameof(DisableProtection), protectionDuration);
        }
    }

    private IEnumerator Blinking()
    {
        while (isProtected)
        {
            objectRenderer.enabled = false;
            objectCollider.enabled = false;
            yield return blinkWait;
            objectRenderer.enabled = true;
            objectCollider.enabled = true;
            yield return blinkWait;
        }
    }

    private void DisableProtection()
    {
        isProtected = false;
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
    }

    public bool IsProtected()
    {
        return isProtected;
    }
}