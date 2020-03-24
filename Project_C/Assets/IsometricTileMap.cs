using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IsometricTileMapEditor))]
public class IsometricTileMap : MonoBehaviour
{
    Dictionary<Vector3Int, GameObject> _tileMap;
    GameObject _tileMapPivotObject;

    void Start()
    {
        _tileMap = new Dictionary<Vector3Int, GameObject>();
        _tileMapPivotObject = new GameObject("TileMap");
        _tileMapPivotObject.transform.position = Vector3.zero;
        _tileMapPivotObject.transform.rotation = Quaternion.identity;
        _tileMapPivotObject.AddComponent<IsometricTransform>().position = new Vector3(0f, -Isometric.IsometricTileSize.y, 0f);
    }

    public bool ContainsTile(Vector3 isoPos)
    {
        Vector3Int index = EffectiveUtility.IsoPositionToIndex(isoPos);
        return ContainsTile(index);
    }

    public bool ContainsTile(Vector3Int index)
    {
        return _tileMap.ContainsKey(index);
    }

    public void AddTile(Vector3 isoPos, GameObject go)
    {
        Vector3Int index = EffectiveUtility.IsoPositionToIndex(isoPos);
        if(!ContainsTile(index))
        {
            GameObject instance = Instantiate(go, Vector3.zero, Quaternion.identity, _tileMapPivotObject.transform);
            IsometricTransform itrasform = instance.GetComponent<IsometricTransform>();
            itrasform.position = isoPos;
            _tileMap.Add(index, instance);
        }
    }

    public void RemoveTile(Vector3 isoPos)
    {
        Vector3Int index = EffectiveUtility.IsoPositionToIndex(isoPos);
        if (ContainsTile(index))
        {
            GameObject instance = _tileMap[index];
            Destroy(instance);
            _tileMap.Remove(index);
        }
    }
}
