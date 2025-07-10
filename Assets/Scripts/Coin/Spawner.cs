using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _respawnDelay = 3f;
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private List<SpawnPoint> _spawnPoints = new();

    private void Start()
    {
        ValidateSpawnPoints();
        InitialSpawn();
    }

    private void ValidateSpawnPoints()
    {
        foreach (var point in _spawnPoints)
        {
            if (point == null)
                Debug.LogError("ќбнаружена пуста€ точка спавна в списке!");
        }
    }

    private void InitialSpawn()
    {
        foreach (var point in _spawnPoints)
        {
            if (point != null)
                SpawnCoin(point);
        }
    }

    private void SpawnCoin(SpawnPoint point)
    {
        Coin newCoin = Instantiate(_coinPrefab, point.transform.position, Quaternion.identity);
        newCoin.OriginPoint = point;
        newCoin.Collected += OnCoinCollected;
    }

    private void OnCoinCollected(CollectibleItem item)
    {
        if (item is Coin coin)
        {
            coin.Collected -= OnCoinCollected;
            StartCoroutine(RespawnProcess(coin.OriginPoint));
            Destroy(coin.gameObject);
        }
    }

    private IEnumerator RespawnProcess(SpawnPoint point)
    {
        yield return new WaitForSeconds(_respawnDelay);
        SpawnCoin(point);
    }

    private void OnValidate()
    {
        if (_coinPrefab == null)
            Debug.LogError("Coin Prefab не назначен!", this);

        if (_spawnPoints.Count == 0)
            Debug.LogError("Ќе добавлены точки спавна!", this);
    }
}