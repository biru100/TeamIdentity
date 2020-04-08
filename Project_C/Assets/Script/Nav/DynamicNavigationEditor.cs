using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using UnityEditor.AI;


[CanEditMultipleObjects]
[CustomEditor(typeof(DynamicNavigation))]
public class DynamicNavigationEditor : Editor
{
    SerializedProperty m_AgentTypeID;
    SerializedProperty m_LayerMask;
    SerializedProperty m_UseGeometry;

    public readonly GUIContent LayerMaskContent = new GUIContent("Include Layers");

    void OnEnable()
    {
        m_AgentTypeID = serializedObject.FindProperty("m_AgentTypeID");
        m_LayerMask = serializedObject.FindProperty("m_LayerMask");
        m_UseGeometry = serializedObject.FindProperty("m_UseGeometry");

        NavMeshVisualizationSettings.showNavigation++;
    }

    void OnDisable()
    {
        NavMeshVisualizationSettings.showNavigation--;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var bs = NavMesh.GetSettingsByID(m_AgentTypeID.intValue);

        if (bs.agentTypeID != -1)
        {
            // Draw image
            const float diagramHeight = 80.0f;
            Rect agentDiagramRect = EditorGUILayout.GetControlRect(false, diagramHeight);
            NavMeshEditorHelpers.DrawAgentDiagram(agentDiagramRect, bs.agentRadius, bs.agentHeight, bs.agentClimb, bs.agentSlope);
        }

        AgentTypePopup("Agent Type", m_AgentTypeID);

        EditorGUILayout.PropertyField(m_LayerMask, LayerMaskContent);
        EditorGUILayout.PropertyField(m_UseGeometry);
    }

    public static void AgentTypePopup(string labelName, SerializedProperty agentTypeID)
    {
        var index = -1;
        var count = NavMesh.GetSettingsCount();
        var agentTypeNames = new string[count + 2];
        for (var i = 0; i < count; i++)
        {
            var id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            var name = NavMesh.GetSettingsNameFromID(id);
            agentTypeNames[i] = name;
            if (id == agentTypeID.intValue)
                index = i;
        }
        agentTypeNames[count] = "";
        agentTypeNames[count + 1] = "Open Agent Settings...";

        bool validAgentType = index != -1;
        if (!validAgentType)
        {
            EditorGUILayout.HelpBox("Agent Type invalid.", MessageType.Warning);
        }

        var rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
        EditorGUI.BeginProperty(rect, GUIContent.none, agentTypeID);

        EditorGUI.BeginChangeCheck();
        index = EditorGUI.Popup(rect, labelName, index, agentTypeNames);
        if (EditorGUI.EndChangeCheck())
        {
            if (index >= 0 && index < count)
            {
                var id = NavMesh.GetSettingsByIndex(index).agentTypeID;
                agentTypeID.intValue = id;
            }
            else if (index == count + 1)
            {
                NavMeshEditorHelpers.OpenAgentSettings(-1);
            }
        }

        EditorGUI.EndProperty();
    }

}
