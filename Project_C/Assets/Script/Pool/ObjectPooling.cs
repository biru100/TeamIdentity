using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPooling : BehaviorSingleton<ObjectPooling>
{
    protected Dictionary<Type, List<object>> ObjectPool { get; set; }

    protected override void Init()
    {
        base.Init();
        ObjectPool = new Dictionary<Type, List<object>>();
    }
    
    public static void PushObject<T>(T obj)
    {
        if(Instance.ObjectPool.ContainsKey(typeof(T)))
        {

        }
    }
}
