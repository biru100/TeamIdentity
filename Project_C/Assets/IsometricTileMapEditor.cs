using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EditorMode
{
    E_PLACE,
    E_DELETE,
    E_LENGTH 
}

[RequireComponent(typeof(IsometricTileMap))]
public class IsometricTileMapEditor : MonoBehaviour
{
    public EditorMode currentEditorMode = EditorMode.E_PLACE;
    public int brushHeight = -1;
    public GameObject currentTile;

    IsometricTileMap tileManager;

    // Start is called before the first frame update

    void Start()
    {
        tileManager = GetComponent<IsometricTileMap>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentEditorMode = (EditorMode)(((int)currentEditorMode + 1) % (int)EditorMode.E_LENGTH);
        }

        if(Input.GetKeyDown(KeyCode.Minus))
        {
            brushHeight--;
        }
        else if(Input.GetKeyDown(KeyCode.Equals))
        {
            brushHeight++;
        }

        if(Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 currentMouseTilePos = Isometric.GetOwnedTilePos(
                        Isometric.GetIsometicBasePositionByWorldRay(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                        Camera.main.transform.forward));
            currentMouseTilePos.y = brushHeight * Isometric.IsometricTileSize.y;

            switch (currentEditorMode)
            {
                case EditorMode.E_PLACE:
                    {
                        if (currentTile != null)
                        {
                            tileManager.AddTile(currentMouseTilePos, currentTile);
                        }
                    }
                    break;
                case EditorMode.E_DELETE:
                    {
                        tileManager.RemoveTile(currentMouseTilePos);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
