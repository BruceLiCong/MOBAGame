﻿using UnityEngine;

public class Singleton<T> : MonoBehaviour
    where T : MonoBehaviour
{
    private static T _Instance;

    public static T Instance
    {
        get
        {
            return _Instance;
        }
    }

    protected virtual void Awake()
    {
        _Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        _Instance = null;
    }
}
