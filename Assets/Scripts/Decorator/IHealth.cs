using System;

namespace Decorator
{
    public interface IHealth
    {
        void TakeDamage(int damage);
        int GetCurrentHealth();
        event Action OnDeath;
    }  
}

