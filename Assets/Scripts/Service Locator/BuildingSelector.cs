using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingSelector : MonoBehaviour, IBuildingSelector
{
    private Transform[] buildings;

    private void Awake()
    {
        var buildingsParent  = GameObject.Find("Buildings"); //Why noty passing this through inspector?
        if (buildingsParent != null)
        {
            buildings = new Transform[buildingsParent.transform.childCount];
            for (int i = 0; i < buildingsParent.transform.childCount; i++)
            {
                buildings[i] = buildingsParent.transform.GetChild(i);
            }
        }
        else
        {
            Debug.LogError("Buildings GameObject not found in the scene.");
        }
        
        ServiceLocator.RegisterService<IBuildingSelector>(this);
    }
    
    public Transform GetTargetBuilding()
    {
        if (buildings.Length == 0) return null;
        
        return buildings[Random.Range(0, buildings.Length)];
    }
}
