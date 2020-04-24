using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using StateBehavior.Node;

[CustomEditor(typeof(StateBehavior.Node.NodeScript))]
public class NodeScriptEditor : Editor
{
    NodeScript script;

    public void OnEnable()
    {
        script = (NodeScript)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("CharacterName");
        script.userName = EditorGUILayout.TextField(script.userName);

        EditorGUILayout.LabelField("StateName");
        script.stateName = EditorGUILayout.TextField(script.stateName);

        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("");

        EditorUtility.SetDirty(script);

        if (GUILayout.Button("Compile Code"))
        {
            StateCompiler.CompileNodeScript(script);
            AssetDatabase.Refresh();
            //CompilationPipeline.RequestScriptCompilation();
        }

        if (GUILayout.Button("Hot Compile Code(VisualStudio User Use Only)"))
        {
            StateCompiler.CompileNodeScript(script);
        }
    }
}
