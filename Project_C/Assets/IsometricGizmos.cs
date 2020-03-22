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

        for (int x = -10; x < 10; ++x)
        {
            Vector3 left = new Vector3((x + 0.5f) * Isometric._isometricTileSize.x, 0f, -9.5f * Isometric._isometricTileSize.y), 
                right = new Vector3((x + 0.5f) * Isometric._isometricTileSize.x, 0f, 9.5f * Isometric._isometricTileSize.y);

            Gizmos.DrawLine(Isometric.IsometricToWorldRotation * left, Isometric.IsometricToWorldRotation * right);
        }

        for (int z = -10; z < 10; ++z)
        {
            Vector3 down = new Vector3(-9.5f * Isometric._isometricTileSize.x, 0f, (z + 0.5f) * Isometric._isometricTileSize.y), 
                top = new Vector3(9.5f * Isometric._isometricTileSize.x, 0f, (z + 0.5f) * Isometric._isometricTileSize.y);

            Gizmos.DrawLine(Isometric.IsometricToWorldRotation * down, Isometric.IsometricToWorldRotation * top);
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
