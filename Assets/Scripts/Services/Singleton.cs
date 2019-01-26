using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To anyone readying this, don't be like me, don't do this. This is bad! bad! bad!
/// </summary>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public T instance { set; get; }

    protected void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            OnAwake();
        }
    }

    protected virtual void OnAwake()
    {

    }
}
