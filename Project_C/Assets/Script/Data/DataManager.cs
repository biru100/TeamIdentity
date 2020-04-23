using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Reflection;

public class DataManager : BehaviorSingleton<DataManager>
{
    Dictionary<Type, Dictionary<int, object>> AllDatas { get; set; }

    public static void LoadData()
    {
        Instance.AllDatas = new Dictionary<Type, Dictionary<int, object>>();
        TextAsset[] assets = Resources.LoadAll<TextAsset>("DataTable/");

        foreach(var txtAsset in assets)
        {
            Dictionary<int, object> dataTable = new Dictionary<int, object>();

            Type tableClass = Type.GetType(txtAsset.name);

            Instance.AllDatas.Add(tableClass, dataTable);

            StringReader sr = new StringReader(txtAsset.text);

            while (true)
            {
                string line = sr.ReadLine();

                if (string.IsNullOrEmpty(line))
                    break;

                string[] parts = line.Split('\t');

                object tableElements = tableClass.GetMethod("Load", BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[] { parts });
                dataTable.Add(int.Parse(parts[0]), tableElements);
            }
        }
    }

    public static T GetData<T>(int index)
    {
        return (T)Instance.AllDatas[typeof(T)][index];
    }

    public static List<T> GetDatas<T>()
    {
        return Instance.AllDatas[typeof(T)].Values.Cast<T>().ToList();
    }
}
