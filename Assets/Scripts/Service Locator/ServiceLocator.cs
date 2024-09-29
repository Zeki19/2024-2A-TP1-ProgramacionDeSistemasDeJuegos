using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

    public static void RegisterService<T>(T service)
    {
        var type = typeof(T);
        if (!Services.ContainsKey(type))
        {
            Services.Add(type, service);
        }
    }

    public static T GetService<T>()
    {
        var type = typeof(T);
        if (Services.ContainsKey(type))
        {
            return (T)Services[type];
        }

        throw new Exception($"Service of type {type} is not registered.");
    }

    public static void UnregisterService<T>()
    {
        var type = typeof(T);
        if (Services.ContainsKey(type))
        {
            Services.Remove(type);
        }
    }
}
