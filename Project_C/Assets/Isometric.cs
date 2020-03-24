using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[InitializeOnLoad]
public class Isometric
{
    public static Vector3 IsometricTileSize = Vector3.one;

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
            IsometricTileSize = new Vector3(float.Parse(tileSize[0]), float.Parse(tileSize[1]),
                float.Parse(tileSize[2]));
        }
    }

    public static void SaveConfig()
    {
        File.WriteAllText(Application.dataPath + "/IsomectricConfig.txt", IsometricTileSize.x + "\t" +
            IsometricTileSize.y + "\t" +
            IsometricTileSize.z);
    }

    public static Quaternion WorldToIsometricRotation { get; private set; }

    public static Quaternion IsometricToWorldRotation { get; private set; }

    public static Vector3 TranslationIsometricToScreen(Vector3 isoPosition)
    {
        Vector3 worldPos = IsometricToWorldRotation * isoPosition;
        worldPos.z = 0f;
        return worldPos;
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
        float tileX = IsometricTileSize.x * Mathf.Floor((pos.x + 0.5f * IsometricTileSize.x) / IsometricTileSize.x);
        float tileY = IsometricTileSize.y * Mathf.Floor((pos.y + 0.5f * IsometricTileSize.y) / IsometricTileSize.y);
        float tileZ = IsometricTileSize.z * Mathf.Floor((pos.z + 0.5f * IsometricTileSize.z) / IsometricTileSize.z);

        return new Vector3(tileX, tileY, tileZ);
    }
}
