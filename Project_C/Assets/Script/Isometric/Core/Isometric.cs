using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class Isometric
{
    Vector3 isometricTileSize = Vector3.one;
    Vector2 isometricRenderSize = Vector2.one;

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
        q = worldToIso;

        Matrix4x4 mat = new Matrix4x4();

        mat[0, 0] = 1 - 2 * (q.y * q.y + q.z * q.z);
        mat[1, 0] = 2 * (q.x * q.y - q.z * q.w);
        mat[2, 0] = 2 * (q.x * q.z + q.y * q.w);
        mat[0, 1] = 2 * (q.x * q.y + q.z * q.w);
        mat[1, 1] = 1 - 2 * (q.x * q.x + q.z * q.z);
        mat[2, 1] = 2 * (q.y * q.z - q.x * q.w);
        mat[0, 2] = 2 * (q.x * q.z - q.y * q.w);
        mat[1, 2] = 2 * (q.y * q.z + q.x * q.w);
        mat[2, 2] = 1 - 2 * (q.x * q.x + q.y * q.y);
        mat[3, 0] = mat[3, 1] = mat[3, 2] = mat[0, 3] = mat[1, 3] = mat[2, 3] = 0;
        mat[3, 3] = 1;


        File.WriteAllText(Application.dataPath + "/Resources/Config/IsometricToWorld.txt", isoToWorld.x + "\t" +
            isoToWorld.y + "\t" +
            isoToWorld.z + "\t" +
            isoToWorld.w);

        File.WriteAllText(Application.dataPath + "/Resources/Config/WorldToIsometric.txt", worldToIso.x + "\t" +
            worldToIso.y + "\t" +
            worldToIso.z + "\t" +
            worldToIso.w);

        File.WriteAllText(Application.dataPath + "/Resources/Config/IsometricToWorldMatrix.txt", mat.ToString());

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
