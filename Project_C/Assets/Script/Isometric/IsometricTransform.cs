using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IsometricTransform : MonoBehaviour
{
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public Vector3 localScale = Vector3.one;


    private void Update()
    {
        if(!Application.IsPlaying(gameObject))
            TranslateIsometricToWorldCoordination();
    }

    private void LateUpdate()
    {
        TranslateIsometricToWorldCoordination();
    }

    void TranslateIsometricToWorldCoordination()
    {
        transform.position = Isometric.IsometricToWorldRotation * position;
        transform.rotation = rotation;
        transform.localScale = localScale;
    }
}
