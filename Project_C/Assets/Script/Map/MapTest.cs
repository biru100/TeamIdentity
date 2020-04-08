using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(IsometricTileMap))]
public class MapTest : MonoBehaviour
{
    [SerializeField] string _mapDataName;

    // Start is called before the first frame update
    void Start()
    {
        string jsonData = ResourceManager.GetResource<TextAsset>("Map/" + _mapDataName).text;
        TileMapData data = JsonUtility.FromJson<TileMapData>(jsonData);
        GetComponent<IsometricTileMap>().FromJson(data.mapData);
        DynamicNavigation.Instance.BuildNavigation();

        Debug.Log(Type.GetType(typeof(void).FullName).IsClass);
        Debug.Log(typeof(void).Name);
        Debug.Log(Type.GetType(typeof(Character).FullName).IsClass);
        Debug.Log(typeof(Character).Name);
        Debug.Log(Type.GetType(typeof(float).FullName).IsClass);
        Debug.Log(typeof(float).Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
