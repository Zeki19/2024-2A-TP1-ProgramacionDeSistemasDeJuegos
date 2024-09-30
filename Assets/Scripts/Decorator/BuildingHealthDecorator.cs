using System;
using Decorator;
using UnityEngine;

public class BuildingHealthDecorator : HealthDecorator
{
    private readonly GameObject _buildingGameObject;

    public BuildingHealthDecorator(IHealth health, GameObject buildingGameObject) : base(health)
    {
        _buildingGameObject = buildingGameObject;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (GetCurrentHealth() <= 0)
        {
            DestroyBuilding();
        }
    }

    private void DestroyBuilding()
    {
        GameObject.Destroy(_buildingGameObject);
    }
}
