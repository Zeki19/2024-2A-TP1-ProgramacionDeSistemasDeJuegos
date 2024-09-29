using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] public NavMeshAgent agent;
        private EnemyPool _enemyPool;
        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };
    
        private void Reset() => FetchComponents();

        private void Awake() => FetchComponents();
    
        private void FetchComponents()
        {
            agent ??= GetComponent<NavMeshAgent>();
        }

        public void Initialize(EnemyPool pool)
        {
            _enemyPool = pool;
        }

        private void OnEnable()
        {
            SetTargetBuilding();
        }
        
        private void SetTargetBuilding()
        {
            // Get the building selector service from the Service Locator
            var buildingSelector = ServiceLocator.GetService<IBuildingSelector>();

            if (buildingSelector == null)
            {
                Debug.LogError($"{name}: Building selector service not found!");
                return;
            }

            Transform targetBuilding = buildingSelector.GetTargetBuilding();
            if (targetBuilding != null)
            {
                agent.SetDestination(targetBuilding.position);
                StartCoroutine(AlertSpawn());
            }
            else
            {
                Debug.LogError($"{name}: No valid target building found!");
            }
        }

        private IEnumerator AlertSpawn()
        {
            //Waiting one frame because event subscribers could run their onEnable after us.
            yield return null;
            OnSpawn();
        }

        private void Update()
        {
            if (agent.hasPath
                && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
            {
                Debug.Log($"{name}: I'll die for my people!");
                Die();
            }
        }

        private void Die()
        {
            OnDeath();
            if (_enemyPool != null)
            {
                _enemyPool.ReturnEnemyToPool(gameObject);
            }
            else
            {
                Destroy(gameObject); 
            }
            
        }
    }
}
