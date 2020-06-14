using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(RendererSupportComponent))]
public class RendererSupportComponentEditor : Editor
{
    SerializedProperty _IsStatic;
    SerializedProperty _RendererSupportType;
    SerializedProperty _NormalMap;
    SerializedProperty _DepthMap;

    SerializedProperty _TileLength;
    SerializedProperty _SpriteYSize;
    SerializedProperty _DepthColorMultiplier;

    void OnEnable()
    {
        _IsStatic = serializedObject.FindProperty("_isStatic");
        _RendererSupportType = serializedObject.FindProperty("_supportType");
        _NormalMap = serializedObject.FindProperty("_normalMap");
        _DepthMap = serializedObject.FindProperty("_depthTexture");

        _TileLength = serializedObject.FindProperty("_tileLength");
        _SpriteYSize = serializedObject.FindProperty("_spriteYSize");
        _DepthColorMultiplier = serializedObject.FindProperty("_depthColorMultiplier");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_IsStatic);
        EditorGUILayout.PropertyField(_RendererSupportType);

        if ((RendererSupportType)_RendererSupportType.enumValueIndex == RendererSupportType.E_Character)
        {
            EditorGUILayout.PropertyField(_NormalMap);
        }
        else if ((RendererSupportType)_RendererSupportType.enumValueIndex == RendererSupportType.E_DepthShaderLit)
        {
            EditorGUILayout.PropertyField(_NormalMap);
            EditorGUILayout.PropertyField(_DepthMap);
            EditorGUILayout.PropertyField(_TileLength);
            EditorGUILayout.PropertyField(_SpriteYSize);
            EditorGUILayout.PropertyField(_DepthColorMultiplier);
        }
        else if ((RendererSupportType)_RendererSupportType.enumValueIndex == RendererSupportType.E_DepthShader)
        {
            EditorGUILayout.PropertyField(_DepthMap);
            EditorGUILayout.PropertyField(_TileLength);
            EditorGUILayout.PropertyField(_SpriteYSize);
            EditorGUILayout.PropertyField(_DepthColorMultiplier);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
