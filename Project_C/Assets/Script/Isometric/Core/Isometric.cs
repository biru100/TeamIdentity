using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class Isometric
{
    Vector3 isometricTileSize = Vector3.one;
    Vector2 isometricRenderSize = Vector2.one;

    float isometricZtileDistance;
    float isometricYTileDistance;
    float isometricStandZTileDistance;
    float isometricStandYSpriteSize;

    public static Vector3 IsometricTileSize
    {
        get => instance.isometricTileSize;
        set => instance.isometricTileSize = value;
    }

    public static Vector2 IsometricRenderSize
    {
        get => instance.isometricRenderSize;
        set => instance.isometricRenderSize = value;
    }

    public static float IsometricGridSize { get => instance.isometricTileSize.x; }
    public static float IsometricZTileDistance { get => instance.isometricZtileDistance; }
    public static float IsometricStandZTileDistance { get => instance.isometricStandZTileDistance; }
    public static float IsometricStandYSpriteSize { get => instance.isometricStandYSpriteSize; }

    private static Isometric _instance = null;

    public static Isometric instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Isometric();
                UpdateConfig();
            }
            return _instance;
        }
    }

    private Isometric()
    {
        
    }

    public static void CalcIsometricRotation(Vector2 renderSize)
    {
        Quaternion q = Quaternion.AngleAxis(-Mathf.Rad2Deg * Mathf.Asin(renderSize.y / renderSize.x), new Vector3(1f, 0f, 1f).normalized);
        Quaternion q1 = Quaternion.Euler(0f, 45f, 0f);

        Quaternion isoToWorld = q1 * q;
        Quaternion worldToIso = Quaternion.Inverse(isoToWorld);

        File.WriteAllText(Application.dataPath + "/Resources/Config/IsometricToWorld.txt", isoToWorld.x + "\t" +
            isoToWorld.y + "\t" +
            isoToWorld.z + "\t" +
            isoToWorld.w);

        File.WriteAllText(Application.dataPath + "/Resources/Config/WorldToIsometric.txt", worldToIso.x + "\t" +
            worldToIso.y + "\t" +
            worldToIso.z + "\t" +
            worldToIso.w);

        UpdateConfig();
    }

    public static void UpdateConfig()
    {
        string[] quaternion = ResourceManager.GetResource<TextAsset>("Config/IsometricToWorld").text.Split('\t');
        IsometricToWorldRotation = new Quaternion(float.Parse(quaternion[0]), float.Parse(quaternion[1]),
            float.Parse(quaternion[2]), float.Parse(quaternion[3]));

        quaternion = ResourceManager.GetResource<TextAsset>("Config/WorldToIsometric").text.Split('\t');
        WorldToIsometricRotation = new Quaternion(float.Parse(quaternion[0]), float.Parse(quaternion[1]),
            float.Parse(quaternion[2]), float.Parse(quaternion[3]));

        string[] tileSize = ResourceManager.GetResource<TextAsset>("Config/IsomectricConfig").text.Split('\t');
        _instance.isometricTileSize = new Vector3(float.Parse(tileSize[0]), float.Parse(tileSize[1]),
            float.Parse(tileSize[2]));

        _instance.isometricRenderSize = new Vector2(float.Parse(tileSize[3]), float.Parse(tileSize[4]));


        Vector3 offset = IsometricToWorldRotation * new Vector3(-1f, 0f, 1f) * _instance.isometricTileSize.x - IsometricToWorldRotation * Vector3.zero;
        _instance.isometricZtileDistance = offset.z;

        offset = IsometricToWorldRotation * Vector3.up * _instance.isometricTileSize.x - IsometricToWorldRotation * Vector3.zero;
        _instance.isometricStandYSpriteSize = offset.y;
        _instance.isometricStandZTileDistance = offset.z;
    }

    public static void SaveConfig()
    {
        File.WriteAllText(Application.dataPath + "/IsomectricConfig.txt", IsometricTileSize.x + "\t" +
            IsometricTileSize.y + "\t" +
            IsometricTileSize.z + "\t" +
            IsometricRenderSize.x + "\t" +
            IsometricRenderSize.y);
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
