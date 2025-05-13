using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider2D))]
public class Invincibility : MonoBehaviour
{
    [SerializeField] private float _protectionDuration = 2f;
    [SerializeField] private float _blinkSpeed = 0.2f;
    
    private const string PlayerLayer = "Player";
    private const string EnemyLayer = "Enemy";

    private bool _isProtected = false;
    private Renderer _objectRenderer;
    private Collider2D _objectCollider;
    private WaitForSeconds _blinkWait;   
    private int _playerLayerIndex;
    private int _enemyLayerIndex;

    private void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        _objectCollider = GetComponent<Collider2D>();
        _blinkWait = new WaitForSeconds(_blinkSpeed);
        CacheLayerIndices();
    }

    private void CacheLayerIndices()
    {
        _playerLayerIndex = LayerMask.NameToLayer(PlayerLayer);
        _enemyLayerIndex = LayerMask.NameToLayer(EnemyLayer);
    }

    public void MakeProtected()
    {
        if (_isProtected == false)
        {
            _isProtected = true;
            StartCoroutine(Blinking());
            SetLayerCollision(true);
            Invoke(nameof(DisableProtection), _protectionDuration);
        }
    }

    private void SetLayerCollision(bool ignore)
    {
        Physics2D.IgnoreLayerCollision(_playerLayerIndex,_enemyLayerIndex,ignore);
    }

    public bool IsProtected()
    {
        return _isProtected;
    }

    private IEnumerator Blinking()
    {
        while (_isProtected)
        {
            _objectRenderer.enabled = _objectRenderer.enabled == false;
            yield return _blinkWait;
        }
        _objectRenderer.enabled = true;
    }

    private void DisableProtection()
    {
        _isProtected = false;
        SetLayerCollision(false);
        StopCoroutine(Blinking());
    }
}