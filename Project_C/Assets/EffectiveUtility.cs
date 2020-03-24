using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EffectiveUtility
{
    public static Vector3 VectorMultiple(Vector3 l, Vector3 r)
    {
        return new Vector3(l.x * r.x, l.y * r.y, l.z * r.z);
    }

    public static Vector3Int IsoPositionToIndex(Vector3 isoPos)
    {
        return new Vector3Int((int)(isoPos.x / Isometric.IsometricTileSize.x),
            (int)(isoPos.y / Isometric.IsometricTileSize.y),
            (int)(isoPos.z / Isometric.IsometricTileSize.z));
    }

    public static Vector3 RoundPixelPerfect(Vector3 worldPos)
    {
        return new Vector3(Mathf.Round(worldPos.x * 100f) * 0.01f,
            Mathf.Round(worldPos.y * 100f) * 0.01f,
            Mathf.Round(worldPos.z * 100f) * 0.01f);
    }
}
