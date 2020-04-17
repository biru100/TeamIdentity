using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TilePreviewElement : MonoBehaviour
{
    GameObject _tile = null;

    public GameObject tile {
        get
        {
            return _tile;
        }
        set
        {
            if(_tile != value)
            {
                _tile = value;
                CreatePreview();
            }
        }
    }

    Sprite tilePreviewSprite;

    Text tileName;
    Image tilePreview;

    private void Awake()
    {
        tileName = GetComponentInChildren<Text>();
        tilePreview = GetComponentsInChildren<Image>()[1];

        GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<IsometricTileMapEditor>().CurrentTile = _tile);
    }

    void CreatePreview()
    {
        if (tilePreviewSprite != null)
            Destroy(tilePreviewSprite);

        tilePreviewSprite = _tile.GetComponentInChildren<SpriteRenderer>().sprite;//Sprite.Create(AssetPreview.GetAssetPreview(_tile), new Rect(Vector2.zero, Vector2.one * 100f), Vector2.one * 0.5f, 100f);
        tilePreview.sprite = tilePreviewSprite;

        tileName.text = _tile.name;
    }
}
