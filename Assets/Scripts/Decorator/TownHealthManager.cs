using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Decorator;
using UnityEditor;

public class TownHealthManager : MonoBehaviour
{
    public static TownHealthManager Instance { get; private set; }
    
    private Dictionary<Vector3, IHealth> buildingsHealthDictionary = new Dictionary<Vector3, IHealth>();
    [SerializeField] private int maxHealth = 1000;

    public event Action<Vector3> OnBuildingDestroyed;

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
        
        foreach (Transform child in transform)
        {
            Health health = new Health(maxHealth);
            
            IHealth buildingHealth = new BuildingHealthDecorator(health, child.gameObject);
            
            buildingsHealthDictionary[child.position] = buildingHealth;
            
            buildingHealth.OnDeath += () => HandleBuildingDestruction(child);
        }
    }
    
    private void HandleBuildingDestruction(Transform building)
    {
        buildingsHealthDictionary.Remove(building.position);
        OnBuildingDestroyed?.Invoke(building.position);
        
        Debug.Log($"{building.name} has been destroyed!");
        Destroy(building.gameObject);
    }
    
    public void TakeDamage(Vector3 buildingPosition, int damage)
    {
        if (buildingsHealthDictionary.TryGetValue(buildingPosition, out IHealth buildingHealth))
        {
            buildingHealth.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning($"Building with position {buildingPosition} not found!");
        }
    }
    
    public bool IsBuildingAlive(Vector3 buildingPosition)
    {
        return buildingsHealthDictionary.ContainsKey(buildingPosition);
    }

    private void Update()
    {
        if (transform.childCount < 1)
        {
            EndGame();
        }
    }
    
    public void EndGame()
    {
        Debug.Log("Game Over! Stopping the game...");
        EditorApplication.isPlaying = false;
    }
}
