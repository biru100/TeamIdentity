using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(IsometricTileMap))]
public class MapTest : MonoBehaviour
{
    [SerializeField] string _mapDataName;
    [SerializeField] int _testCost;
    [SerializeField] int _testDraw;

    // Start is called before the first frame update
    void Start()
    {
        string jsonData = ResourceManager.GetResource<TextAsset>("Map/" + _mapDataName).text;
        TileMapData data = JsonUtility.FromJson<TileMapData>(jsonData);
        GetComponent<IsometricTileMap>().FromJson(data.mapData, true);
        DynamicNavigation.Instance.SetNavMeshData(DynamicNavigation.Instance.BuildNavigation(null));
        PlayerStatus.CurrentStatus.CurrentManaCost = _testCost;
        InGameInterface.Instance.DrawCard(_testDraw);

        //Player player = Instantiate(ResourceManager.GetResource<GameObject>("Tiles/Player")).GetComponent<Player>();
        //player.transform.position = CurrentRoom.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
