using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
