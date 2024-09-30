using System;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyVfx : MonoBehaviour
    {
        private Enemy _enemy;

        private void Reset() => FetchComponents();

        private void Awake() => FetchComponents();

        private void FetchComponents()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void OnEnable()
        {
            _enemy.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            _enemy.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        { 
            var particleClone = ParticleManager.Instance.GetRandomParticleClone();
            if (particleClone != null)
            {
                particleClone.transform.position = transform.position;
                particleClone.gameObject.SetActive(true);
                particleClone.Play();
            }
        }
    }
}