using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public interface IPoolObject
{
    void ClearMember();
}

public class ObjectPooling : BehaviorSingleton<ObjectPooling>
{
    [SerializeField] protected Dictionary<Type, List<IPoolObject>> ObjectPool { get; set; }

    protected override void Init()
    {
        base.Init();
        ObjectPool = new Dictionary<Type, List<IPoolObject>>();
    }
    
    public static void PushObject(IPoolObject obj)
    {
        obj.ClearMember();
        if (Instance.ObjectPool.ContainsKey(obj.GetType()))
        {
            Instance.ObjectPool[obj.GetType()].Add(obj);
        }
        else
        {
            Instance.ObjectPool.Add(obj.GetType(), new List<IPoolObject>() { obj });
        }
    }

    public static T PopObject<T>() where T : class, IPoolObject
    {
        if (Instance.ObjectPool.ContainsKey(typeof(T)))
        {
            List<IPoolObject> poolingObjects = Instance.ObjectPool[typeof(T)];
            T retObj = poolingObjects[0] as T;
            poolingObjects.RemoveAt(0);

            if (poolingObjects.Count == 0)
                Instance.ObjectPool.Remove(typeof(T));

            return retObj;
        }
        else
        {
            T retObj = Activator.CreateInstance<T>();//(T)Activator.CreateInstance(typeof(T), BindingFlags.Public, null, parameters, null);
            return retObj;
        }
    }
}
