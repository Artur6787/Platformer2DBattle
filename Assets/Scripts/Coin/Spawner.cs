using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _respawnDelay = 3f;
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private List<SpawnPoint> _spawnPoints = new();

    private Dictionary<Coin, SpawnPoint> _coinSpawnPoints = new Dictionary<Coin, SpawnPoint>();

    private void Start()
    {
        ValidateSpawnPoints();
        InitialSpawn();
    }

    private void OnValidate()
    {
        if (_coinPrefab == null)
            Debug.LogError("Coin Prefab �� ��������!", this);

        if (_spawnPoints.Count == 0)
            Debug.LogError("�� ��������� ����� ������!", this);
    }

    private IEnumerator RespawnProcess(SpawnPoint point)
    {
        yield return new WaitForSeconds(_respawnDelay);
        SpawnCoin(point);
    }

    private void ValidateSpawnPoints()
    {
        bool _hasNull = false;

        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i] == null)
            {
                Debug.LogError($"���������� ������ ����� ������ � ������ ��� �������� {i}!", this);
                _hasNull = true;
            }
        }

        if (_hasNull)
        {
            Debug.LogError("������: ������� ������ ����� ������. ������� ����� ��������.", this);
            this.enabled = false;
        }
    }

    private void InitialSpawn()
    {
        foreach (var point in _spawnPoints)
        {
                SpawnCoin(point);
        }
    }

    private void SpawnCoin(SpawnPoint point)
    {
        Coin newCoin = Instantiate(_coinPrefab, point.transform.position, Quaternion.identity);
        newCoin.Collected += OnCoinCollected;
        _coinSpawnPoints[newCoin] = point;
    }

    private void OnCoinCollected(CollectibleItem item)
    {
        if (item is Coin coin)
        {
            coin.Collected -= OnCoinCollected;

            if (_coinSpawnPoints.TryGetValue(coin, out var spawnPoint))
            {
                StartCoroutine(RespawnProcess(spawnPoint));
                _coinSpawnPoints.Remove(coin);
            }

            Destroy(coin.gameObject);
        }
    }
}