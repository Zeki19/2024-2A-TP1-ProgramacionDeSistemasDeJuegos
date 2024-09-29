using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialPoolSize = 200;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemyFromPool(Vector3 position, Quaternion rotation)
    {
        GameObject enemy;
        if (enemyPool.Count > 0)
        {
            enemy = enemyPool.Dequeue();
        }
        else
        {
            enemy = Instantiate(enemyPrefab, transform);
        }

        enemy.transform.position = position;
        enemy.transform.rotation = rotation;
        enemy.SetActive(true);
        
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.Initialize(this);
        }

        return enemy;
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
