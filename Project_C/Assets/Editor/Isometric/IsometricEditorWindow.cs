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

        GUILayout.Label("TileSize_X : " + Isometric.IsometricTileSize.x);
        GUILayout.Label("TileSize_Y : " + Isometric.IsometricTileSize.y);
        GUILayout.Label("TileSize_Z : " + Isometric.IsometricTileSize.z);

        float x = EditorGUILayout.FloatField("RenderSize_X", Isometric.IsometricRenderSize.x);
        float y = EditorGUILayout.FloatField("RenderSize_Y", Isometric.IsometricRenderSize.y);

        Isometric.IsometricRenderSize = new Vector2(x, y);

        if (GUILayout.Button("Calc Isometric Rotation"))
        {
            float tileSize = Isometric.IsometricRenderSize.x / Mathf.Sqrt(2);
            Isometric.IsometricTileSize = Vector3.one * tileSize;
            Isometric.SaveConfig();
            Isometric.CalcIsometricRotation(Isometric.IsometricRenderSize);
        }
    }
}
