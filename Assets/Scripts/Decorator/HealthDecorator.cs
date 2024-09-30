using System;

namespace Decorator
{
    public abstract class HealthDecorator : IHealth
    {
        private Action _onDeath;

        public event Action OnDeath
        {
            add { _onDeath += value; }
            remove { _onDeath -= value; }
        }

        protected void InvokeOnDeath()
        {
            _onDeath?.Invoke();
        }

        private readonly IHealth _health;

        public HealthDecorator(IHealth health)
        {
            _health = health;
        }

        public virtual void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
            if (_health.GetCurrentHealth() <= 0)
            {
                InvokeOnDeath();
            }
        }

        public int GetCurrentHealth()
        {
            return _health.GetCurrentHealth();
        }
    }
}