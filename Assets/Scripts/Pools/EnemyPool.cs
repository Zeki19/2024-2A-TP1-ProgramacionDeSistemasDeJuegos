using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private Transform[] spawners;
        [SerializeField] private int initialPoolSize = 200;

        private readonly Queue<GameObject> _enemyPool = new Queue<GameObject>();

        private void Awake()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                Transform randomSpawner = spawners[Random.Range(0, spawners.Length)];
                GameObject enemy = Instantiate(enemyPrefab, randomSpawner.position, Quaternion.identity, transform);
                enemy.SetActive(false);
                _enemyPool.Enqueue(enemy);
            }
        }

        public GameObject GetEnemyFromPool(Vector3 position, Quaternion rotation)
        {
            GameObject enemy;
            if (_enemyPool.Count > 0)
            {
                enemy = _enemyPool.Dequeue();
            }
            else
            {
                Transform randomSpawner = spawners[Random.Range(0, spawners.Length)];
                enemy = Instantiate(enemyPrefab, randomSpawner.position, Quaternion.identity, transform);
            }
            enemy.SetActive(true);
            return enemy;
        }

        public void ReturnEnemyToPool(GameObject enemy)
        {
            enemy.SetActive(false);
            _enemyPool.Enqueue(enemy);
        }
    }
}