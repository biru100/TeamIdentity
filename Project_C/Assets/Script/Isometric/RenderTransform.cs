using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderTransform : MonoBehaviour
{
    public Vector3 imageOffset = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;

    public float z_weight = 0f;

    private void Update()
    {
        if(!Application.isPlaying)
            TranslateIsometricToWorldCoordination();
    }

    private void LateUpdate()
    {
        TranslateIsometricToWorldCoordination();
    }

    public Vector3 GetIsometricPosition()
    {
        TranslateIsometricToWorldCoordination();
        return transform.position;
    }

    public void TranslateIsometricToWorldCoordination()
    {
        transform.position = Isometric.IsometricToWorldRotation * transform.parent.position
            + imageOffset
            + Vector3.forward * Isometric.IsometricTileSize.z * z_weight * 0.5f;
        
        transform.position = new Vector3(Mathf.Round(transform.position.x * 100f) * 0.01f
        , Mathf.Round(transform.position.y * 100f) * 0.01f, transform.position.z);

        transform.rotation = rotation;
    }
}
