using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingDataManager : BehaviorSingleton<PlayingDataManager>
{
    public List<DataFilterModule> PlayingDataFilterModules { get; set; }

    protected override void Init()
    {
        base.Init();
        PlayingDataFilterModules = new List<DataFilterModule>();

        //add fixed module

        PlayingDataFilterModules.Add(new UseCardsCountFilterModule());
    }

    public static void ReferencingLog(BaseLogData logData)
    {
        foreach(var module in Instance.PlayingDataFilterModules)
        {
            module.InspectData(logData);
        }
    }

    public static T GetModule<T>() where T : DataFilterModule
    {
        int index = Instance.PlayingDataFilterModules.FindIndex((m) => m.GetType() == typeof(T));
        if (index > 0)
        {
            return Instance.PlayingDataFilterModules[index] as T;
        }

        return null;
    }

    public static void AddModule<T>() where T : DataFilterModule, new()
    {
        if(Instance.PlayingDataFilterModules.FindIndex((m)=>m.GetType() == typeof(T)) < 0)
        {
            Instance.PlayingDataFilterModules.Add(new T());
        }
    }

    public static void RemoveModule<T>() where T : DataFilterModule, new()
    {
        int index = Instance.PlayingDataFilterModules.FindIndex((m) => m.GetType() == typeof(T));
        if (index > 0)
        {
            Instance.PlayingDataFilterModules.RemoveAt(index);
        }
    }
}
