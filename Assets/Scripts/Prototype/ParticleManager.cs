using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

    [SerializeField] private ParticleSystem[] particlePrefabs;

    private Dictionary<string, ParticleSystem> _particles;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        _particles = new Dictionary<string, ParticleSystem>();
        foreach (var prefab in particlePrefabs)
        {
            if (prefab != null)
            {
                _particles[prefab.name] = prefab;
            }
        }
    }
    
    public ParticleSystem GetRandomParticleClone()
    {
        var particleArray = new List<ParticleSystem>(_particles.Values);
        ParticleSystem selectedPrototype = particleArray[UnityEngine.Random.Range(0, particleArray.Count)];

        return Instantiate(selectedPrototype);
    }
}
