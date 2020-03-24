using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[InitializeOnLoad]
public class Isometric
{
    public static Vector3 _isometricTileSize = Vector3.one;

    static Isometric()
    {
        UpdateConfig();
    }

    public static void UpdateConfig()
    {
        if (File.Exists(Application.dataPath + "/IsometricToWorld.txt"))
        {
            string[] quaternion = File.ReadAllText(Application.dataPath + "/IsometricToWorld.txt").Split('\t');
            IsometricToWorldRotation = new Quaternion(float.Parse(quaternion[0]), float.Parse(quaternion[1]),
                float.Parse(quaternion[2]), float.Parse(quaternion[3]));
        }

        if (File.Exists(Application.dataPath + "/WorldToIsometric.txt"))
        {
            string[] quaternion = File.ReadAllText(Application.dataPath + "/WorldToIsometric.txt").Split('\t');
            WorldToIsometricRotation = new Quaternion(float.Parse(quaternion[0]), float.Parse(quaternion[1]),
                float.Parse(quaternion[2]), float.Parse(quaternion[3]));
        }

        if (File.Exists(Application.dataPath + "/IsomectricConfig.txt"))
        {
            string[] tileSize = File.ReadAllText(Application.dataPath + "/IsomectricConfig.txt").Split('\t');
            _isometricTileSize = new Vector3(float.Parse(tileSize[0]), float.Parse(tileSize[1]),
                float.Parse(tileSize[2]));
        }
    }

    public static void SaveConfig()
    {
        File.WriteAllText(Application.dataPath + "/IsomectricConfig.txt", _isometricTileSize.x + "\t" +
            _isometricTileSize.y + "\t" +
            _isometricTileSize.z);
    }

    public static Quaternion WorldToIsometricRotation { get; private set; }

    public static Quaternion IsometricToWorldRotation { get; private set; }

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
