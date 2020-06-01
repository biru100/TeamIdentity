using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class MovementSetPair
{
    public float time;
    public Vector2Int movePixel;
}

[Serializable, CreateAssetMenu(fileName = "New Movement Set", menuName = "Movement Set/Pixel Movement Set")]
public class MovementSet : ScriptableObject
{
    public static MovementSet GetMovementSet(string path)
    {
        return ResourceManager.GetResource<MovementSet>(path);
    }

    [SerializeField] protected List<MovementSetPair> _movementList;


    public List<MovementSetPair> MovementList { get => _movementList; set => _movementList = value; }
}

public class MovementSetController : IPoolObject
{
    public static MovementSetController GetMovementSet(string path)
    {
        return ObjectPooling.PopObject<MovementSetController>().SetData(MovementSet.GetMovementSet(path));
    }

    public static MovementSetController GetMovementSetByAngle(Quaternion rotation, string path)
    {
        return ObjectPooling.PopObject<MovementSetController>().SetData(
            MovementSet.GetMovementSet(path + "_" + AnimUtil.GetRenderAngle(rotation)));
    }

    protected MovementSet data;
    protected float elapsedTime;
    protected int Offset;

    public MovementSetController SetData(MovementSet set)
    {
        data = set;
        return this;
    }

    public bool GetMovementData(float deltaTime, out Vector3 delta)
    {
        elapsedTime += deltaTime;

        if(data.MovementList.Count > Offset && elapsedTime >= data.MovementList[Offset].time)
        {
            Vector2Int deltaPixel = data.MovementList[Offset].movePixel;
            delta = new Vector3(Isometric.IsometricGridSize, 0f, Isometric.IsometricGridSize) * deltaPixel.x * 0.01f / Isometric.IsometricRenderSize.x
                + new Vector3(-Isometric.IsometricGridSize, 0f, Isometric.IsometricGridSize) * deltaPixel.y * 0.01f / Isometric.IsometricRenderSize.y;

            Offset++;
            return true;
        }
        else
        {
            delta = Vector3.zero;
            return false;
        }
    }

    public void ClearMember()
    {
        data = null;
        elapsedTime = 0f;
        Offset = 0;
    }

    public void Destroy()
    {
        ObjectPooling.PushObject(this);
    }
}
