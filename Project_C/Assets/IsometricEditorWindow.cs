using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class IsometricEditorWindow : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Isometric")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        IsometricEditorWindow window = (IsometricEditorWindow)EditorWindow.GetWindow(typeof(IsometricEditorWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        Isometric._isometricTileSize.x = EditorGUILayout.FloatField("TileSize_X", Isometric._isometricTileSize.x);
        Isometric._isometricTileSize.y = EditorGUILayout.FloatField("TileSize_Y", Isometric._isometricTileSize.y);
    }
}
