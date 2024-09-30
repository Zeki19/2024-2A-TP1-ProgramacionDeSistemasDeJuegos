using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingSelector : MonoBehaviour, IBuildingSelector
{
    private List<Transform> _buildings = new List<Transform>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _buildings.Add(transform.GetChild(i));
        }
        
        ServiceLocator.RegisterService<IBuildingSelector>(this);
        
        if (TownHealthManager.Instance != null)
        {
            TownHealthManager.Instance.OnBuildingDestroyed += OnBuildingDestroyed;
        }
    }
    
    public Transform GetTargetBuilding()
    {
        if (_buildings.Count == 0) return null;
        
        return _buildings[Random.Range(0, _buildings.Count)];
    }
    private void OnBuildingDestroyed(Vector3 destroyedBuildingPosition)
    {
        _buildings.RemoveAll(building => building != null && building.position == destroyedBuildingPosition);
    }
}
