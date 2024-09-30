using System.Collections.Generic;
using UnityEngine;

namespace Decorator
{
    public class EnemyHealthManager : MonoBehaviour
    {
        public static EnemyHealthManager Instance { get; private set; } //Singleton

        [SerializeField] private int initialHealth = 100;
        [SerializeField] private int damagePerSecond = 1;

        private List<IHealth> enemies = new List<IHealth>();
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            InvokeRepeating(nameof(ReduceHealthForEnemies), 1f, 1f);
        }
        
        private void ReduceHealthForEnemies()
        {
            foreach (var enemy in enemies)
            {
                if (enemy is MonoBehaviour enemyComponent && enemyComponent.gameObject.activeInHierarchy)
                {
                    enemy.TakeDamage(damagePerSecond);
                }
            }
        }

        public void RegisterEnemy(IHealth enemy)
        {
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }

        public void UnregisterEnemy(IHealth enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
        }

        public int GetInitialHealth()
        {
            return initialHealth;
        }
    }
}
