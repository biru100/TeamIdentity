using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderTransform : MonoBehaviour
{
    public Vector3 imageOffset = Vector3.zero;

    public float z_weight = 0f;

    private void Update()
    {
        TranslateIsometricToWorldCoordination();
    }

    //private void LateUpdate()
    //{
    //    TranslateIsometricToWorldCoordination();
    //}

    void TranslateIsometricToWorldCoordination()
    {
        transform.position = Isometric.IsometricToWorldRotation * transform.parent.position
            + imageOffset
            + Vector3.forward * Isometric.IsometricTileSize.z * z_weight * 0.5f;
        transform.rotation = Quaternion.identity;
    }
}
