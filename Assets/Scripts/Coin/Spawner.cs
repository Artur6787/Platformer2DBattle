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
                Debug.LogError("Обнаружена пустая точка спавна в списке!");
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
            Debug.LogError("Не добавлены точки спавна!", this);
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Spawner : MonoBehaviour
//{
//    [SerializeField] private float _spawnTime;
//    [SerializeField] private Coin _coinPrefab;
//    [SerializeField] private List<SpawnPoint> _spawnPoints;

//    private bool _isWorking = true;

//    private void OnValidate()
//    {
//        if (_coinPrefab == null)
//        {
//            Debug.LogError("Префаб монеты не назначен в инспекторе!");
//        }

//        if (_spawnPoints.Count == 0)
//        {
//            Debug.LogError("Нет назначенных точек спавна в инспекторе!");
//        }
//    }

//    private void Start()
//    {
//        StartCoroutine(Spawn());
//    }

//    private IEnumerator Spawn()
//    {
//        WaitForSeconds time = new WaitForSeconds(_spawnTime);

//        while (_isWorking)
//        {
//            foreach (var spawnPoint in _spawnPoints)
//            {
//                Coin coin = Instantiate(_coinPrefab, spawnPoint.transform.position, Quaternion.identity);
//                coin.Destroyed += OnCoinDestroyed;
//            }

//            yield return time;
//        }
//    }

//    private void OnCoinDestroyed(CollectibleItem destroyedCoin)
//    {
//        Debug.Log("Монета уничтожена: " + destroyedCoin.name);
//    }
//}