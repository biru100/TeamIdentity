using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPoolMonoBehavior
{
    void OnSleep();
    void OnAwake();
}

public class GameObjectPooling : BehaviorSingleton<GameObjectPooling>
{
    Dictionary<Type, List<GameObject>> GameObjectPool { get; set; }


    protected override void Init()
    {
        base.Init();
        GameObjectPool = new Dictionary<Type, List<GameObject>>();
    }

    public static void PushObject<T>(T component) where T : MonoBehaviour, IPoolMonoBehavior
    {
        component.OnSleep();
        if (Instance.GameObjectPool.ContainsKey(component.GetType()))
        {
            Instance.GameObjectPool[component.GetType()].Add(component.gameObject);
        }
        else
        {
            Instance.GameObjectPool.Add(component.GetType(), new List<GameObject>() { component.gameObject });
        }
    }

    public static T PopObject<T>() where T : MonoBehaviour, IPoolMonoBehavior
    {
        if (Instance.GameObjectPool.ContainsKey(typeof(T)))
        {
            List<GameObject> poolingObjects = Instance.GameObjectPool[typeof(T)];
            T retObj = poolingObjects[0].GetComponent<T>() as T;
            poolingObjects.RemoveAt(0);

            if (poolingObjects.Count == 0)
                Instance.GameObjectPool.Remove(typeof(T));

            return retObj;
        }
        else
        {
            T retObj = Activator.CreateInstance<T>();
            //(T)Activator.CreateInstance(typeof(T), BindingFlags.Public, null, parameters, null);
            return retObj;
        }
    }
}
