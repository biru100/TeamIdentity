using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum MapWay
{
    E_NONE = 0,
    E_LEFT = 1 << 0,
    E_TOP = 1 << 1,
    E_RIGHT = 1 << 2,
    E_BOTTOM = 1 << 3
}

[Serializable]
public class TileMapData
{
    [SerializeField] public string mapName = "none";
    [SerializeField] public string mapTheme = "none";
    [SerializeField] public int mapDifficulty = 0;
    [SerializeField] public int mapWeight = 0;
    [SerializeField] public Vector3Int mapMin = Vector3Int.zero;
    [SerializeField] public Vector3Int mapMax = Vector3Int.zero;
    [SerializeField] public TileData mapData;
    [SerializeField] public int mapWay = 0;
}
