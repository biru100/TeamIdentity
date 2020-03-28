using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
