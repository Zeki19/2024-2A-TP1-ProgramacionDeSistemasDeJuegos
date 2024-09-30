using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePrototype : MonoBehaviour, IPrototype<ParticlePrototype>
{
    public ParticlePrototype Clone()
    {
        return Instantiate(this);
    }
}
