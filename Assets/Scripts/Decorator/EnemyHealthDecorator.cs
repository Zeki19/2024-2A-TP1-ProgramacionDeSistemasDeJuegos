using System.Collections;
using System.Collections.Generic;
using Decorator;
using UnityEngine;

public class EnemyHealthDecorator : HealthDecorator
{
    private readonly MonoBehaviour _owner;
    
    public EnemyHealthDecorator(IHealth health, MonoBehaviour owner) : base(health)
    {
        _owner = owner;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (GetCurrentHealth() <= 0)
        {
            InvokeOnDeath();
        }
    }
}
