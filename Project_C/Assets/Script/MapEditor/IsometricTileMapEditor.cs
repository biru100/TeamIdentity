using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    [SerializeField] private GameObject currentTile;

    Transform previewTileTransform;

    public GameObject CurrentTile
    {
        get
        {
            return currentTile;
        }
        set
        {
            if(currentTile != value)
            {
                UpdatePreviewBrush(value);
                currentTile = value;
            }
        }
    }

    IsometricTileMap tileManager;

    Vector3 currentMouseTilePos;

    public Vector3Int CurrentMouseTileIndex
    {
        get
        {
            return EffectiveUtility.IsoPositionToIndex(currentMouseTilePos);
        }
    }

    void UpdatePreviewBrush(GameObject newBrush)
    {
        if(previewTileTransform != null)
        {
            Destroy(previewTileTransform.gameObject);
            previewTileTransform = null;
        }

        previewTileTransform = Instantiate(newBrush).transform;
        previewTileTransform.gameObject.SetActive(!(currentEditorMode > 0));
    }

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
            if(previewTileTransform != null)
            {
                previewTileTransform.gameObject.SetActive(!(currentEditorMode > 0));
            }
        }

        if(Input.GetKeyDown(KeyCode.Minus))
        {
            brushHeight--;
        }
        else if(Input.GetKeyDown(KeyCode.Equals))
        {
            brushHeight++;
        }

        currentMouseTilePos = Isometric.GetOwnedTilePos(
            Isometric.GetIsometicBasePositionByWorldRay(Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Camera.main.transform.forward));
        currentMouseTilePos.y = brushHeight * Isometric.IsometricTileSize.y;

        if(previewTileTransform != null)
        {
            previewTileTransform.position = currentMouseTilePos;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

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

        if (Input.GetKey(KeyCode.Mouse1))
        {
            GameObject tile = tileManager.GetTile(currentMouseTilePos);
            if (tile != null)
            {
                CurrentTile = tile;
            }
        }
    }
}
