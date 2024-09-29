using System;

namespace HealthSystem
{
    public class Health
    {
        public int MaxHP { get; private set; }
        public int CurrentHP { get; private set; }

        public event Action OnDeath;

        public Health(int maxHealth)
        {
            MaxHP = maxHealth;
            CurrentHP = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP <= 0 )
            {
                CurrentHP = 0;
                OnDeath?.Invoke();
            }
        }

        public int GetCurrentHealth()
        {
            return CurrentHP;
        }
    }
}