﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileIndexStringPair
{
    public Vector3Int index;
    public string tag;

    public TileIndexStringPair(Vector3Int index, string tag)
    {
        this.index = index;
        this.tag = tag;
    }
}

[Serializable]
public class TileData
{
    public List<TileIndexStringPair> data;
    public TileData(List<TileIndexStringPair> data)
    {
        this.data = data;
    }
}

public class IsometricTileMap : MonoBehaviour
{
    Dictionary<Vector3Int, GameObject> _tileMap = new Dictionary<Vector3Int, GameObject>();
    GameObject _tileMapPivotObject;

    public Vector3Int min { get; private set; }
    public Vector3Int max { get; private set; }

    void Start()
    {
        if (_tileMapPivotObject == null)
        {
            CreatePivot();
        }
    }

    void CreatePivot()
    {
        _tileMapPivotObject = new GameObject("TileMap");
        _tileMapPivotObject.transform.position = Vector3.zero;
        _tileMapPivotObject.transform.rotation = Quaternion.identity;
        _tileMapPivotObject.transform.position = Vector3.zero;
        _tileMapPivotObject.AddComponent<TileTag>().Tag = "TileMap";
    }

    void DestroyMap()
    {
        _tileMap.Clear();
        Destroy(_tileMapPivotObject);
        if (_tileMapPivotObject != null) _tileMapPivotObject = null;
        min = Vector3Int.zero;
        max = Vector3Int.zero;
    }

    public TileData ToJson()
    {
        List<TileIndexStringPair> data = new List<TileIndexStringPair>();
        foreach (var iter in _tileMap)
        {
            data.Add(new TileIndexStringPair(iter.Key, iter.Value.GetComponentInChildren<TileTag>().Tag));
        }

        TileData dataOBJ = new TileData(data);

        return dataOBJ;
    }

    public void FromJson(TileData data, bool activateLogic = false)
    {
        DestroyMap();
        CreatePivot();

        foreach (var pair in data.data)
        {
            AddTile(EffectiveUtility.VectorMultiple(new Vector3(pair.index.x, pair.index.y, pair.index.z), Isometric.IsometricTileSize), 
                ResourceManager.GetResource<GameObject>("Tiles/" + pair.tag), activateLogic);
        }
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

    public void AddTile(Vector3 isoPos, GameObject go, bool activateLogic = false)
    {
        Vector3Int index = EffectiveUtility.IsoPositionToIndex(isoPos);
        if(!ContainsTile(index))
        {
            if(_tileMap.Count == 0)
            {
                min = index;
                max = index;
            }
            else
            {
                min = EffectiveUtility.Min(min, index);
                max = EffectiveUtility.Min(max, index);
            }

            GameObject instance = Instantiate(go, isoPos, Quaternion.identity, _tileMapPivotObject.transform);
            if(instance.GetComponent<Character>())
                instance.GetComponent<Character>().enabled = activateLogic;
            _tileMap.Add(index, instance);
        }
    }

    public GameObject GetTile(Vector3 isoPos)
    {
        Vector3Int index = EffectiveUtility.IsoPositionToIndex(isoPos);
        if (ContainsTile(index))
            return _tileMap[index];
        else
            return null;
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
