using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class IsometricGizmos : MonoBehaviour
{
    public Material lineMat;

    Vector3 lastMousePos;

    public bool activateGrid = true;
    public bool activateHoverGuideLine = true;

    public void Start()
    {
        lastMousePos = new Vector3();
    }

    private void OnPostRender()
    {
        if (!lineMat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

        GL.Begin(GL.LINES);
        lineMat.SetPass(0);

        Vector3 currentCameraCenterTilePos = Isometric.GetOwnedTilePos(
            Isometric.GetIsometicBasePositionByWorldRay(Camera.main.transform.position, Camera.main.transform.forward));

        Vector3 currentMouseTilePos = Isometric.GetOwnedTilePos(
            Isometric.GetIsometicBasePositionByWorldRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward));


        if (activateGrid)
        {
            GL.Color(Color.white);

            for (int x = -10; x < 10; ++x)
            {
                Vector3 left = new Vector3((x + 0.5f) * Isometric.IsometricTileSize.x, 0f
                    , -9.5f * Isometric.IsometricTileSize.z),
                    right = new Vector3((x + 0.5f) * Isometric.IsometricTileSize.x, 0f,
                    9.5f * Isometric.IsometricTileSize.z);

                GL.Vertex(Isometric.TranslationIsometricToScreen(left + currentCameraCenterTilePos));
                GL.Vertex(Isometric.TranslationIsometricToScreen(right + currentCameraCenterTilePos));
            }

            for (int z = -10; z < 10; ++z)
            {
                Vector3 down = new Vector3(-9.5f * Isometric.IsometricTileSize.x, 0f,
                    (z + 0.5f) * Isometric.IsometricTileSize.z),
                    top = new Vector3(9.5f * Isometric.IsometricTileSize.x, 0f,
                    (z + 0.5f) * Isometric.IsometricTileSize.z);

                GL.Vertex(Isometric.TranslationIsometricToScreen(down + currentCameraCenterTilePos));
                GL.Vertex(Isometric.TranslationIsometricToScreen(top + currentCameraCenterTilePos));
            }
        }


        if (activateHoverGuideLine)
        {
            GL.Color(Color.red);

            Vector3 leftBottom = currentMouseTilePos - 0.5f * new Vector3(Isometric.IsometricTileSize.x, 0f, Isometric.IsometricTileSize.z); ;
            Vector3 leftTop = currentMouseTilePos + 0.5f * new Vector3(-Isometric.IsometricTileSize.x, 0f, Isometric.IsometricTileSize.z);
            Vector3 rightBottom = currentMouseTilePos + 0.5f * new Vector3(Isometric.IsometricTileSize.x, 0f, -Isometric.IsometricTileSize.z);
            Vector3 rightTop = currentMouseTilePos + 0.5f * new Vector3(Isometric.IsometricTileSize.x, 0f, Isometric.IsometricTileSize.z); ;

            GL.Vertex(Isometric.TranslationIsometricToScreen(leftBottom));
            GL.Vertex(Isometric.TranslationIsometricToScreen(leftTop));

            GL.Vertex(Isometric.TranslationIsometricToScreen(leftBottom));
            GL.Vertex(Isometric.TranslationIsometricToScreen(rightBottom));

            GL.Vertex(Isometric.TranslationIsometricToScreen(leftTop));
            GL.Vertex(Isometric.TranslationIsometricToScreen(rightTop));

            GL.Vertex(Isometric.TranslationIsometricToScreen(rightBottom));
            GL.Vertex(Isometric.TranslationIsometricToScreen(rightTop));
        }
        GL.End();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            transform.position += (lastMousePos - Input.mousePosition) * 0.007f;
        }

        if (!EventSystem.current.IsPointerOverGameObject() && Input.mouseScrollDelta.y != 0)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * 0.1f;
        }

        lastMousePos = Input.mousePosition;
    }
}
