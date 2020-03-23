using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IsometricGizmos : MonoBehaviour
{
    Vector3 _mousePoint;


    public void OnDrawGizmos()
    {
        var view = SceneView.currentDrawingSceneView;

        if(view != null && view.camera != null)
        {
            Camera sceneCamera = view.camera;

            Vector3 mousePosition = Event.current.mousePosition;
            mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
            mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
            mousePosition.y = -mousePosition.y;



            Vector3 currentCameraCenterTilePos = Isometric.GetOwnedTilePos(
                Isometric.GetIsometicBasePositionByWorldRay(sceneCamera.transform.position, sceneCamera.transform.forward));

            for (int x = -10; x < 10; ++x)
            {
                Vector3 left = new Vector3((x + 0.5f) * Isometric._isometricTileSize.x, 0f
                    , -9.5f * Isometric._isometricTileSize.z),
                    right = new Vector3((x + 0.5f) * Isometric._isometricTileSize.x, 0f, 
                    9.5f * Isometric._isometricTileSize.z);

                Gizmos.DrawLine(Isometric.IsometricToWorldRotation * (left + currentCameraCenterTilePos)
                    , Isometric.IsometricToWorldRotation * (right + currentCameraCenterTilePos));
            }

            for (int z = -10; z < 10; ++z)
            {
                Vector3 down = new Vector3(-9.5f * Isometric._isometricTileSize.x, 0f, 
                    (z + 0.5f) * Isometric._isometricTileSize.z),
                    top = new Vector3(9.5f * Isometric._isometricTileSize.x, 0f, 
                    (z + 0.5f) * Isometric._isometricTileSize.z);

                Gizmos.DrawLine(Isometric.IsometricToWorldRotation * (down + currentCameraCenterTilePos), 
                    Isometric.IsometricToWorldRotation * (top + currentCameraCenterTilePos));
            }

            Vector3 leftBottom = currentCameraCenterTilePos - 0.5f * new Vector3(Isometric._isometricTileSize.x, 0f, Isometric._isometricTileSize.z); ;
            Vector3 leftTop = currentCameraCenterTilePos + 0.5f * new Vector3(-Isometric._isometricTileSize.x, 0f, Isometric._isometricTileSize.z);
            Vector3 rightBottom = currentCameraCenterTilePos + 0.5f * new Vector3(Isometric._isometricTileSize.x, 0f, -Isometric._isometricTileSize.z);
            Vector3 rightTop = currentCameraCenterTilePos + 0.5f * new Vector3(Isometric._isometricTileSize.x, 0f, Isometric._isometricTileSize.z); ;

            Gizmos.color = new Color(1f, 0f, 0f);

            Gizmos.DrawSphere(Isometric.IsometricToWorldRotation * currentCameraCenterTilePos, 0.05f);

            Gizmos.DrawLine(Isometric.IsometricToWorldRotation * leftBottom, Isometric.IsometricToWorldRotation * leftTop);
            Gizmos.DrawLine(Isometric.IsometricToWorldRotation * leftBottom, Isometric.IsometricToWorldRotation * rightBottom);
            Gizmos.DrawLine(Isometric.IsometricToWorldRotation * leftTop, Isometric.IsometricToWorldRotation * rightTop);
            Gizmos.DrawLine(Isometric.IsometricToWorldRotation * rightBottom, Isometric.IsometricToWorldRotation * rightTop);

            Gizmos.color = new Color(1f, 1f, 1f);
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
