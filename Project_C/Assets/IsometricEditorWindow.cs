using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class IsometricEditorWindow : EditorWindow
{
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
        Isometric.IsometricTileSize.x = EditorGUILayout.FloatField("TileSize_X", Isometric.IsometricTileSize.x);
        Isometric.IsometricTileSize.y = EditorGUILayout.FloatField("TileSize_Y", Isometric.IsometricTileSize.y);
        Isometric.IsometricTileSize.z = EditorGUILayout.FloatField("TileSize_Z", Isometric.IsometricTileSize.z);

        if (GUILayout.Button("Save"))
        {
            Isometric.SaveConfig();
        }

        if (GUILayout.Button("Update Config"))
        {
            Isometric.UpdateConfig();
        }
    }
}
