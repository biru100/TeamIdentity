using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Isometric
{
    private static readonly Quaternion _isometricToWorldRotation = new Quaternion(0.2798482f, 0.3647052f, 0.1159169f, 0.8804762f);
    private static readonly Quaternion _worldToIsometricRotation = new Quaternion(-0.2798482f, -0.3647052f, -0.1159169f, 0.8804762f);

    public static Vector3 _isometricTileSize = Vector3.one;

    public static Quaternion WorldToIsometricRotation
    {
        get
        {
            return _worldToIsometricRotation;
        }
    }

    public static Quaternion IsometricToWorldRotation
    {
        get
        {
            return _isometricToWorldRotation;
        }
    }

    public static Vector3 GetIsometicBasePositionByWorldRay(Vector3 origin, Vector3 direction)
    {
        Vector3 n = IsometricToWorldRotation * Vector3.up;
        Vector3 p2 = IsometricToWorldRotation * Vector3.zero;

        float t = Vector3.Dot(n, p2 - origin) / Vector3.Dot(n, direction);
        return WorldToIsometricRotation * (origin + (t * direction));
    }

    public static Vector3 GetOwnedTilePos(Vector3 pos)
    {
        float tileX = _isometricTileSize.x * Mathf.Floor((pos.x + 0.5f * _isometricTileSize.x) / _isometricTileSize.x);
        float tileY = _isometricTileSize.y * Mathf.Floor((pos.y + 0.5f * _isometricTileSize.y) / _isometricTileSize.y);
        float tileZ = _isometricTileSize.z * Mathf.Floor((pos.z + 0.5f * _isometricTileSize.z) / _isometricTileSize.z);

        return new Vector3(tileX, tileY, tileZ);
    }
}
