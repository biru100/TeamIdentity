using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorSingleton<SingletonClass> : MonoBehaviour 
    where SingletonClass : BehaviorSingleton<SingletonClass>
{
    protected static SingletonClass _instance;

    public static SingletonClass Instance
    {
        get
        {
            if(_instance == null)
            {
                SingletonClass instance = FindObjectOfType<SingletonClass>();
                if(instance == null)
                    instance = new GameObject(typeof(SingletonClass).Name).AddComponent<SingletonClass>();

                _instance = instance;
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = (SingletonClass)this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
            Destroy(gameObject);      
    }

    protected virtual void OnDestroy()
    {
        if(_instance == this)
        {
            _instance = null;
        }
    }
}
