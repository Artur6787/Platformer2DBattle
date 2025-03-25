using System.Collections;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    [SerializeField] private float protectionDuration = 2f;
    [SerializeField] private float blinkSpeed = 0.1f;

    private bool isProtected = false;
    private Renderer objectRenderer;
    private Collider2D objectCollider;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider2D>();
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
            yield return new WaitForSeconds(blinkSpeed);
            objectRenderer.enabled = true;
            objectCollider.enabled = true;
            yield return new WaitForSeconds(blinkSpeed);
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