using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IsometricGizmos : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        var view = SceneView.currentDrawingSceneView;

        if(view != null && view.camera != null)
        {
            Camera sceneCamera = view.camera;
            //sceneCamera.ScreenToWorldPoint()
        }

        Quaternion isomectricRot = Quaternion.Inverse(Quaternion.LookRotation(new Vector3(-1, 1, 1).normalized, Vector3.up));

        for (int x = -10; x <= 10; ++x)
        {
            Vector3 left = new Vector3(x, -0f, -10f), right = new Vector3(x, 0f, 10f);

            Gizmos.DrawLine(isomectricRot * left, isomectricRot * right);
        }

        for (int z = -10; z <= 10; ++z)
        {
            Vector3 down = new Vector3(-10f, 0f, z), top = new Vector3(10f, 0f, z);

            Gizmos.DrawLine(isomectricRot * down, isomectricRot * top);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
