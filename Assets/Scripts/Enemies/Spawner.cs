using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private int spawnsPerPeriod = 10;
    [SerializeField] private float frequency = 30;
    [SerializeField] private float period = 0;

    private void Awake()
    {
        enemyPool = FindObjectOfType<EnemyPool>();
    }

    private void OnEnable()
    {
        if (frequency > 0) period = 1 / frequency;
    }

    private IEnumerator Start()
    {
        while (!destroyCancellationToken.IsCancellationRequested)
        {
            for (int i = 0; i < spawnsPerPeriod; i++)
            {
                Vector3 spawnPosition = transform.position;
                Quaternion spawnRotation = Quaternion.identity;

                enemyPool.GetEnemyFromPool(spawnPosition, spawnRotation);
            }

            yield return new WaitForSeconds(period);
        }
    }
}
