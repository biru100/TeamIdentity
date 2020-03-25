using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class EditorStateViewer : MonoBehaviour
{
    Text index;
    Text mode;
    IsometricTileMapEditor editor;

    // Start is called before the first frame update
    void Start()
    {
        index = GetComponent<Text>();
        mode = transform.GetChild(0).GetComponent<Text>();
        editor = FindObjectOfType<IsometricTileMapEditor>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int curIndex = editor.CurrentMouseTileIndex;
        index.text = "X = " + curIndex.x + ", Y = " + curIndex.y + ", Z = " + curIndex.z;

        mode.text = editor.currentEditorMode > 0 ? "DELETE MODE" : "PLACE MODE";
    }
}
