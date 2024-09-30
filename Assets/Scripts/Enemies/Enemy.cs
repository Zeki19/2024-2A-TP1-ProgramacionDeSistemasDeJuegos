using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Decorator;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] public NavMeshAgent agent;
        private EnemyPool _enemyPool;
        Vector3 _buildingPosition;
        [SerializeField] private int damageToBuildings = 100;
        private IHealth _health;
        
        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };
    
        private void Reset() => FetchComponents();

        private void Awake()
        {
            FetchComponents();

            _health = new EnemyHealthDecorator(new Health(2), this);
            _health.OnDeath += Die;
        }

        private void FetchComponents()
        {
            agent ??= GetComponent<NavMeshAgent>();
        }
        

        private void OnEnable()
        {
            SetTargetBuilding();
            if (TownHealthManager.Instance != null)
            {
                TownHealthManager.Instance.OnBuildingDestroyed += OnBuildingDestroyed;
            }
            
            if (_health is Health healthComponent)
            {
                healthComponent.ResetHealth();
            }
            
            if (EnemyHealthManager.Instance != null)
            {
                EnemyHealthManager.Instance.RegisterEnemy(_health);
            }
        }
        
        private void OnDisable()
        {
            if (TownHealthManager.Instance != null)
            {
                TownHealthManager.Instance.OnBuildingDestroyed -= OnBuildingDestroyed;
            }

            if (_health != null)
            {
                _health.OnDeath -= Die;
            }
            
            if (EnemyHealthManager.Instance != null)
            {
                EnemyHealthManager.Instance.UnregisterEnemy(_health);
            }
        }
        
        private void SetTargetBuilding()
        {
            var buildingSelector = ServiceLocator.GetService<IBuildingSelector>();
            if (buildingSelector == null)
            {
                Debug.LogError($"{name}: Building selector service not found!");
                return;
            }

            Transform targetBuilding = buildingSelector.GetTargetBuilding();
            if (targetBuilding != null)
            {
                _buildingPosition = targetBuilding.position;
                agent.SetDestination(targetBuilding.position);
                StartCoroutine(AlertSpawn());
            }
            else
            {
                //Debug.Log($"{name}: No valid target building found, stopping.");
                agent.ResetPath();
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
            if (agent.hasPath && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
            {
                Debug.Log($"{name}: I'll die for my people!");
                Die();
            } else if (!TownHealthManager.Instance.IsBuildingAlive(_buildingPosition))
            {
                SetTargetBuilding();
            }
        }

        private void Die()
        {
            if (agent.hasPath && agent.destination != null)
            {
                var townCenter = TownHealthManager.Instance;
                if (townCenter != null && townCenter.IsBuildingAlive(_buildingPosition))
                {
                    townCenter.TakeDamage(_buildingPosition, damageToBuildings);
                }
            }
            
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
        
        private void OnBuildingDestroyed(Vector3 destroyedBuildingPosition)
        {
            if (_buildingPosition == destroyedBuildingPosition)
            {
                SetTargetBuilding();
            }
        }
    }
}
